using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TauCode.Cli.Data;
using TauCode.Cli.Exceptions;
using TauCode.Cli.TextClasses;
using TauCode.Parsing;
using TauCode.Parsing.Nodes;

namespace TauCode.Cli
{
    public abstract class CliAddInBase : ICliAddIn
    {
        #region Fields

        private INode _node;
        private readonly INodeFamily _nodeFamily;
        private readonly List<ICliWorker> _workers;

        #endregion

        #region Constructor

        protected CliAddInBase(string name, string version, bool supportsHelp)
        {
            this.Name = name ?? throw new ArgumentNullException(nameof(name), "Use parameterless constructor for nameless add-in creation.");
            this.Version = version;
            this.SupportsHelp = supportsHelp;

            _nodeFamily = new NodeFamily($"Add-in node family: {this.Name ?? string.Empty}");
            _workers = new List<ICliWorker>();
        }

        protected CliAddInBase()
        {
            _nodeFamily = new NodeFamily("Nameless add-in node family");
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

        private INode BuildNode()
        {
            INode addInNode;

            if (this.Name == null)
            {
                addInNode = new IdleNode(_nodeFamily, null); // todo give it a name
            }
            else
            {
                addInNode = new ExactTextNode(
                    this.Name,
                    TermTextClass.Instance,
                    this.ProcessAddInName,
                    _nodeFamily,
                    null); // todo: give it a name

                addInNode.Properties["add-in-name"] = this.Name;
            }

            var workers = this.CreateWorkers();

            if (workers == null)
            {
                throw new CliException($"'{nameof(CreateWorkers)}' must not return null.");
            }

            if (workers.Count == 0)
            {
                throw new CliException($"'{nameof(CreateWorkers)}' must not return empty collection.");
            }

            var validTypes = workers.All(x => x is CliWorkerBase);
            if (!validTypes)
            {
                throw new CliException($"'{nameof(CreateWorkers)}' must return instances of type '{typeof(CliWorkerBase).FullName}'.");
            }

            if (workers.Any(x => x.Name == null) && workers.Count > 1)
            {
                throw new CliException($"'{nameof(CreateWorkers)}' must return either all workers having non-null name, or exactly one worker with null name.");
            }

            foreach (var worker in workers)
            {
                ((CliWorkerBase)worker).AddIn = this;
            }

            _workers.AddRange(workers);

            foreach (var worker in workers)
            {
                addInNode.EstablishLink(worker.Node);
            }

            return addInNode;
        }

        #endregion

        #region Protected

        protected abstract IReadOnlyList<ICliWorker> CreateWorkers();

        #endregion

        #region ICliAddIn Members

        public ICliHost Host { get; internal set; }

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
    }
}
