using System;
using System.Collections.Generic;
using System.IO;
using TauCode.Cli.Data;
using TauCode.Cli.Descriptors;
using TauCode.Cli.Exceptions;
using TauCode.Parsing;
using TauCode.Parsing.Building;
using TauCode.Parsing.Exceptions;
using TauCode.Parsing.TinyLisp;
using TauCode.Parsing.TinyLisp.Data;

namespace TauCode.Cli
{
    public abstract class CliExecutorBase : CliFunctionalityProviderBase, ICliExecutor
    {
        #region Fields

        private readonly PseudoList _form;

        #endregion

        #region Constructor

        protected CliExecutorBase(
            string grammar,
            string version,
            bool supportsHelp)
            : base(ExtractName(grammar), version, supportsHelp)
        {
            var tinyLispLexer = new TinyLispLexer();
            var tinyLispPseudoReader = new TinyLispPseudoReader();
            var lispTokens = tinyLispLexer.Lexize(grammar);
            _form = tinyLispPseudoReader.Read(lispTokens);

            this.Descriptor = (new CliWorkerDescriptorBuilder(grammar)).Build();

            if (this.Name == null)
            {
                if (this.Version != null)
                {
                    throw new ArgumentException("Nameless worker cannot support version.", nameof(version));
                }

                if (this.SupportsHelp)
                {
                    throw new ArgumentException("Nameless worker cannot support help.", nameof(supportsHelp));
                }
            }

            try
            {
                var helper = new CliWorkerDescriptorBuilder(grammar);
                this.Descriptor = helper.Build();
            }
            catch (CliException)
            {
                // couldn't build descriptor
            }
        }

        #endregion

        #region Overridden

        public override TextWriter Output
        {
            get => this.AddIn.Output;
            set => throw new NotSupportedException($"Use host's '{nameof(Output)}'.");
        }

        public override TextReader Input
        {
            get => this.AddIn.Input;
            set => throw new NotSupportedException($"Use host's '{nameof(Output)}'.");
        }

        protected override string GetHelpImpl()
        {
            return this.Descriptor.GetHelp();
        }

        protected override INode CreateNodeTree()
        {
            var nodeFactory = this.CreateNodeFactory();

            ITreeBuilder builder = new TreeBuilder();
            var node = builder.Build(nodeFactory, _form);

            return node;
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

        #region Protected

        protected virtual string CreateNodeFactoryName()
        {
            return $"Worker node factory. Worker name:'{this.Name}'. Worker type: '{this.GetType().FullName}'.";
        }

        protected virtual CliExecutorNodeFactory CreateNodeFactory()
        {
            return new CliExecutorNodeFactory(this.CreateNodeFactoryName());
        }

        #endregion

        #region Private

        private static string ExtractName(string grammar) => (new CliWorkerDescriptorBuilder(grammar)).Build().Name;

        #endregion

        #region ICliWorker Members

        public ICliAddIn AddIn { get; internal set; }

        public virtual FallbackInterceptedCliException HandleFallback(FallbackNodeAcceptedTokenException ex)
        {
            throw new NotSupportedException($"If you want to support fallbacks, override '{nameof(HandleFallback)}' in your '{nameof(CliExecutorBase)}' implementation.");
        }

        public abstract void Process(IList<CliCommandEntry> entries);

        public CliWorkerDescriptor Descriptor { get; }

        #endregion
    }
}
