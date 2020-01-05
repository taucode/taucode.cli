using System;
using System.IO;
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
        }

        #endregion

        #region Private

        private void ProcessAddInName(ActionNode node, IToken token, IResultAccumulator resultAccumulator)
        {
            throw new NotImplementedException();
        }


        #endregion

        #region Protected

        protected virtual INode BuildNode()
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
                
                var workers = this.CreateWorkers();
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

        public abstract ICliWorker[] CreateWorkers();

        #endregion

        #region ICliFunctionalityProvider Members

        public string Name { get; }
        public TextWriter Output => this.Host.Output;
        public TextReader Input => this.Host.Input;
        public INode Node => _node ?? (_node = this.BuildNode());
        public string Version { get; }
        public bool SupportsHelp { get; }

        public string GetHelp()
        {
            throw new NotImplementedException();
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
