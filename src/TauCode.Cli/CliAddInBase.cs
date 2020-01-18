using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TauCode.Cli.Exceptions;
using TauCode.Cli.TextClasses;
using TauCode.Parsing;
using TauCode.Parsing.Nodes;

namespace TauCode.Cli
{
    public abstract class CliAddInBase : CliFunctionalityProviderBase, ICliAddIn
    {
        #region Fields

        private readonly INodeFamily _nodeFamily;
        private readonly List<ICliWorker> _workers;

        #endregion

        #region Constructor

        protected CliAddInBase(string name, string version, bool supportsHelp)
            : base(name, version, supportsHelp)
        {
            // todo: nameless add-in cannot support version & help

            _nodeFamily = new NodeFamily($"Add-in node family: {this.Name ?? string.Empty}");
            _workers = new List<ICliWorker>();
        }

        protected CliAddInBase()
            : this(null, null, false)
        {
        }

        #endregion

        #region Overridden

        public override TextWriter Output
        {
            get => this.Host.Output;
            set => throw new NotSupportedException(); // todo: message 'use writer of owner'
        }

        public override TextReader Input
        {
            get => this.Host.Input;
            set => throw new NotSupportedException(); // todo: message 'use writer of owner'
        }

        protected override string GetHelpImpl()
        {
            return "todo: add-in help.";
        }

        protected override INode CreateNodeTree()
        {
            INode addInNode;

            if (this.Name == null)
            {
                addInNode = new IdleNode(
                    _nodeFamily,
                    $"Root node of nameless '{this.GetType().FullName}' add-in");
            }
            else
            {
                addInNode = new ExactTextNode(
                    this.Name,
                    TermTextClass.Instance,
                    true,
                    this.ProcessAddInName,
                    _nodeFamily,
                    $"Root node of '{this.Name}' add-in");

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

        protected override void OnNodeCreated()
        {
            if (this.Version != null)
            {
                this.AddVersion();
            }

            if (this.SupportsHelp)
            {
                this.AddHelp();
            }
        }

        #endregion

        #region Private

        private void ProcessAddInName(ActionNode node, IToken token, IResultAccumulator resultAccumulator)
        {
            resultAccumulator.AddAddInCommand(node.Properties["add-in-name"]);
        }

        #endregion

        #region Protected

        protected abstract IReadOnlyList<ICliWorker> CreateWorkers();

        #endregion

        #region ICliAddIn Members

        public ICliHost Host { get; internal set; }

        public IReadOnlyList<ICliWorker> GetWorkers()
        {
            if (_workers.Count == 0)
            {
                var dummy = this.Node;
            }

            return _workers;
        }

        #endregion
    }
}
