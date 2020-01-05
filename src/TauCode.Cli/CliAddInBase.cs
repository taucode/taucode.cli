using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TauCode.Cli.Data;
using TauCode.Cli.TextClasses;
using TauCode.Parsing;
using TauCode.Parsing.Nodes;

namespace TauCode.Cli
{
    // todo clean up
    public abstract class CliAddInBase : ICliAddIn
    {
        #region Fields

        private INode _node;
        private readonly INodeFamily _nodeFamily;
        private readonly List<ICliWorker> _workers;

        #endregion

        #region Constructor

        protected CliAddInBase(ICliHost host, string name, string version, bool supportsHelp)
        {
            // todo : check args? (can be null if the only add-in?)
            this.Host = host ?? throw new ArgumentNullException(nameof(host));
            this.Name = name;
            this.Version = version;
            this.SupportsHelp = supportsHelp;

            _nodeFamily = new NodeFamily($"todo-program-family-name-{this.Name}"); // todo: deal with Name == null.
            _workers = new List<ICliWorker>();
        }

        #endregion

        #region Private

        private void ProcessAddInName(ActionNode node, IToken token, IResultAccumulator resultAccumulator)
        {
            var command = new CliCommand
            {
                AddInName = node.Properties["add-in-name"],
            };

            resultAccumulator.AddResult(command);
        }

        #endregion

        #region Protected

        protected abstract IEnumerable<ICliWorker> CreateWorkers();

        private INode BuildNode() // todo: need 'protected virtual'?
        {
            if (this.Name == null)
            {
                throw new NotImplementedException();
            }
            else
            {
                var addInNode = new ExactTextNode(
                    this.Name,
                    TermTextClass.Instance,
                    this.ProcessAddInName,
                    _nodeFamily,
                    null); // todo: give it a name

                addInNode.Properties["add-in-name"] = this.Name; // todo: deal with Name == null
                
                var workers = this.CreateWorkers().ToList();

                _workers.AddRange(workers);

                foreach (var worker in workers)
                {
                    addInNode.EstablishLink(worker.Node);
                }

                return addInNode;
            }
        }

        #endregion

        #region ICliAddIn Members

        public ICliHost Host { get; }

        public IReadOnlyList<ICliWorker> GetWorkers() => _workers;

        #endregion

        #region ICliFunctionalityProvider Members

        public string Name { get; }

        public TextWriter Output
        {
            get => this.Host.Output;
            set => throw new NotSupportedException(); // todo: message 'use writer of owner'
        }

        public TextReader Input
        {
            get => this.Host.Input;
            set => throw new NotSupportedException(); // todo: message 'use writer of owner'
        }

        //public INode Node => _node ?? (_node = this.BuildNode());
        public INode Node
        {
            get
            {
                if (_node == null)
                {
                    _node = this.BuildNode();

                    if (this.Version != null)
                    {
                        this.AddVersion();
                    }

                    if (this.SupportsHelp)
                    {
                        this.AddHelp();
                    }
                }

                return _node;
            }
        }

        public string Version { get; }

        public bool SupportsHelp { get; }

        public string GetHelp()
        {
            return "todo: add-in help.";
        }

        #endregion


        #region Todo Old Stuff

        //#region Fields

        //private readonly string _version;

        //#endregion

        //#region Constructor

        //protected CliAddInBase(
        //    ICliProgram program,
        //    string name,
        //    string version,
        //    bool supportsHelp)
        //{
        //    // todo checks
        //    this.Program = program;
        //    this.Name = name ?? throw new ArgumentNullException(nameof(name));
        //    this.SupportsHelp = supportsHelp;
        //    _version = version;
        //}


        //#endregion




        //public ICliProgram Program { get; }
        //public string Name { get; }
        //public string Description => throw new NotImplementedException();
        //public bool SupportsHelp { get; }
        ////public string GetVersion() => _version;

        ////public string GetHelp()
        ////{
        ////    throw new NotImplementedException();
        ////}

        //public abstract IReadOnlyList<ICliWorker> CreateWorkers();

        #endregion
    }
}
