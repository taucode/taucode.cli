using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TauCode.Cli.Exceptions;
using TauCode.Cli.Help;
using TauCode.Cli.TextClasses;
using TauCode.Parsing;
using TauCode.Parsing.Nodes;

namespace TauCode.Cli
{
    public abstract class CliAddInBase : CliFunctionalityProviderBase, ICliAddIn
    {
        #region Fields

        private readonly INodeFamily _nodeFamily;
        private readonly List<ICliExecutor> _executors;

        #endregion

        #region Constructor

        protected CliAddInBase(string name, string version, bool supportsHelp)
            : base(name, version, supportsHelp)
        {
            if (name == null)
            {
                if (version != null)
                {
                    throw new ArgumentException("Nameless add-in cannot have version.", nameof(version));
                }
            }

            _nodeFamily = new NodeFamily($"Add-in node family: {this.Name ?? string.Empty}. Add-in type is '{this.GetType().FullName}'.");
            _executors = new List<ICliExecutor>();
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
            set => throw new NotSupportedException($"Use host's '{nameof(Output)}'.");
        }

        public override TextReader Input
        {
            get => this.Host.Input;
            set => throw new NotSupportedException($"Use host's '{nameof(Output)}'.");
        }

        protected override string GetHelpImpl()
        {
            if (this.Name == null)
            {
                throw new NotSupportedException($"Default implementation does not support nameless add-ins. You might need to override '{nameof(GetHelpImpl)}'.");
            }

            var sb = new StringBuilder();
            var description = this.Description ?? $"Add-in '{this.GetType().FullName}' does not have a description.";
            sb.AppendLine(description);

            if (_executors[0].Name == null)
            {
                // got single unnamed executor
                sb.AppendLine(_executors.Single().Descriptor.GetHelp());
            }
            else
            {
                var executors = this.GetExecutors().OrderBy(x => x.Descriptor.Verb).ToList();
                var helpBuilder = new HelpBuilder();
                foreach (var executor in executors)
                {
                    sb.Append(executor.Descriptor.Verb);
                    helpBuilder.WriteHelp(sb, executor.Descriptor.Description, 20, 20);
                }
            }

            return sb.ToString();
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

            var executors = this.CreateExecutors();

            if (executors == null)
            {
                throw new CliException($"'{nameof(CreateExecutors)}' must not return null.");
            }

            if (executors.Count == 0)
            {
                throw new CliException($"'{nameof(CreateExecutors)}' must not return empty collection.");
            }

            var validTypes = executors.All(x => x is CliExecutorBase);
            if (!validTypes)
            {
                throw new CliException($"'{nameof(CreateExecutors)}' must return instances of type '{typeof(CliExecutorBase).FullName}'.");
            }

            if (executors.Any(x => x.Name == null) && executors.Count > 1)
            {
                throw new CliException($"'{nameof(CreateExecutors)}' must return either all executors having non-null name, or exactly one executor with null name.");
            }

            foreach (var executor in executors)
            {
                ((CliExecutorBase)executor).AddIn = this;
            }

            _executors.AddRange(executors);

            foreach (var executor in executors)
            {
                addInNode.EstablishLink(executor.Node);
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

        protected abstract IReadOnlyList<ICliExecutor> CreateExecutors();

        protected virtual ICliContext CreateContext() => CliContextBase.Empty;

        #endregion

        #region ICliAddIn Members

        public ICliHost Host { get; internal set; }

        public IReadOnlyList<ICliExecutor> GetExecutors()
        {
            if (_executors.Count == 0)
            {
                var dummy = this.Node;
            }

            return _executors;
        }

        public string Description { get; protected set; }

        public ICliContext Context { get; protected set; }

        public void InitContext()
        {
            if (this.Context != null)
            {
                throw new NotImplementedException();
            }

            this.Context = this.CreateContext();
        }

        #endregion
    }
}
