using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TauCode.Cli.Data;
using TauCode.Cli.Exceptions;
using TauCode.Parsing;
using TauCode.Parsing.Building;
using TauCode.Parsing.Exceptions;
using TauCode.Parsing.TinyLisp;
using TauCode.Parsing.TinyLisp.Data;

namespace TauCode.Cli
{
    public abstract class CliWorkerBase : CliFunctionalityProviderBase, ICliWorker
    {
        #region Fields

        private readonly string _grammar;
        private readonly PseudoList _form;

        #endregion

        #region Constructor

        protected CliWorkerBase(
            string grammar,
            string version,
            bool supportsHelp)
            : base(ExtractName(grammar), version, supportsHelp)
        {
            _grammar = grammar ?? throw new ArgumentNullException(nameof(grammar));

            var tinyLispLexer = new TinyLispLexer();
            var tinyLispPseudoReader = new TinyLispPseudoReader();
            var lispTokens = tinyLispLexer.Lexize(grammar);
            _form = tinyLispPseudoReader.Read(lispTokens);

            if (this.Name == null)
            {
                if (this.Version != null)
                {
                    throw new CliException("Nameless worker cannot support version.");
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
            if (this.Descriptor == null)
            {
                return _grammar;
            }

            var sb = new StringBuilder();
            var descr = this.Descriptor;
            sb.AppendLine(descr.Description);
            if (descr.UsageSamples.Any())
            {
                sb.AppendLine();
                sb.AppendLine("Usage samples:");
                foreach (var usageSample in descr.UsageSamples)
                {
                    sb.AppendLine(usageSample);
                }
            }

            if (descr.Keys.Any())
            {
                sb.AppendLine();
                sb.AppendLine("Keys:");
                foreach (var key in descr.Keys)
                {
                    sb.Append(string.Join(", ", key.Keys));
                    sb.Append($" <{key.ValueDescriptor.DocSubstitution}>");
                    sb.AppendLine();
                }
            }

            if (descr.Arguments.Any())
            {
                sb.AppendLine();
                sb.AppendLine("Arguments:");
                foreach (var arg in descr.Arguments)
                {
                    sb.AppendLine($"<{arg.DocSubstitution}>");
                }
            }

            if (descr.Options.Any())
            {
                sb.AppendLine();
                sb.AppendLine("Options:");
                foreach (var option in descr.Options)
                {
                    sb.AppendLine(string.Join(", ", option.Options));
                }
            }

            return sb.ToString();
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

        protected virtual CliWorkerNodeFactory CreateNodeFactory()
        {
            return new CliWorkerNodeFactory(this.CreateNodeFactoryName());
        }

        #endregion

        #region Private

        private static string ExtractName(string grammar)
        {
            var tinyLispLexer = new TinyLispLexer();
            var tinyLispPseudoReader = new TinyLispPseudoReader();
            var lispTokens = tinyLispLexer.Lexize(grammar);
            var form = tinyLispPseudoReader.Read(lispTokens);

            var topDefblock = form.Single(x => x.GetSingleArgumentAsBool(":is-top") ?? false);
            var supposedCommandForm = topDefblock.GetFreeArguments().First();
            string name = null;
            if (supposedCommandForm.GetCarSymbolName() == "WORKER")
            {
                name = supposedCommandForm.GetSingleKeywordArgument<Symbol>(":worker-name", true)?.Name;
            }

            return name;
        }

        #endregion

        #region ICliWorker Members

        public ICliAddIn AddIn { get; internal set; }

        public virtual FallbackInterceptedCliException HandleFallback(FallbackNodeAcceptedTokenException ex)
        {
            throw new NotSupportedException($"If you want to support fallbacks, override '{nameof(HandleFallback)}' in your '{nameof(CliWorkerBase)}' implementation.");
        }

        public abstract void Process(IList<CliCommandEntry> entries);

        public CliWorkerDescriptor Descriptor { get; set; }

        #endregion
    }
}
