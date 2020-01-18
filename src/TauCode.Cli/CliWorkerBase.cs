using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TauCode.Cli.Data;
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

        private readonly PseudoList _form;

        #endregion

        #region Constructor

        protected CliWorkerBase(
            string grammar,
            string version,
            bool supportsHelp)
            : base(ExtractName(grammar), version, supportsHelp)
        {
            if (grammar == null)
            {
                throw new ArgumentNullException(nameof(grammar));
            }

            var tinyLispLexer = new TinyLispLexer();
            var tinyLispPseudoReader = new TinyLispPseudoReader();
            var lispTokens = tinyLispLexer.Lexize(grammar);
            _form = tinyLispPseudoReader.Read(lispTokens);

            if (this.Name == null)
            {
                if (this.Version != null)
                {
                    throw new NotImplementedException(); // nameless worker cannot have version
                }

                if (this.SupportsHelp)
                {
                    throw new NotImplementedException(); // nameless worker cannot support help
                }
            }
        }

        #endregion

        #region Overridden

        public override TextWriter Output
        {
            get => this.AddIn.Output;
            set => throw new NotSupportedException(); // todo: message 'use writer of owner'
        }

        public override TextReader Input
        {
            get => this.AddIn.Input;
            set => throw new NotSupportedException(); // todo: message 'use writer of owner'
        }

        protected override string GetHelpImpl()
        {
            return "todo: worker help.";
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
            return $"Todo: worker node factory. Name:'{this.Name}'";
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

        public virtual void HandleFallback(FallbackNodeAcceptedTokenException ex)
        {
            throw new NotImplementedException(); // todo message: override to handle fallbacks.
        }

        public abstract void Process(IList<CliCommandEntry> entries);

        #endregion
    }
}
