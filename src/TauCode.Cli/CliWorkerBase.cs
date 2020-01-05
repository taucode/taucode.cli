using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TauCode.Cli.Data;
using TauCode.Cli.Data.Entries;
using TauCode.Parsing;
using TauCode.Parsing.Building;
using TauCode.Parsing.TinyLisp;
using TauCode.Parsing.TinyLisp.Data;

namespace TauCode.Cli
{
    public abstract class CliWorkerBase : ICliWorker
    {
        #region Fields

        private INode _node;
        private readonly PseudoList _form;

        #endregion

        #region Constructor

        protected CliWorkerBase(
            string grammar,
            string version,
            bool supportsHelp)
        {
            if (grammar == null)
            {
                throw new ArgumentNullException(nameof(grammar));
            }

            var tinyLispLexer = new TinyLispLexer();
            var tinyLispPseudoReader = new TinyLispPseudoReader();
            var lispTokens = tinyLispLexer.Lexize(grammar);
            _form = tinyLispPseudoReader.Read(lispTokens);

            this.Name = this.ExtractName();
            this.Version = version;
            this.SupportsHelp = supportsHelp;
        }

        #endregion

        #region Private

        private string ExtractName()
        {
            var topDefblock = _form.Single(x => x.GetSingleArgumentAsBool(":is-top") ?? false);
            var supposedCommandForm = topDefblock.GetFreeArguments().First();
            string name = null;
            if (supposedCommandForm.GetCarSymbolName() == "WORKER")
            {
                name = supposedCommandForm.GetSingleKeywordArgument<Symbol>(":worker-name").Name;
            }

            return name;
        }

        private INode BuildNode()
        {
            INodeFactory nodeFactory = new CliNodeFactory($"Todo: worker for alias '{this.Name}'");
            IBuilder builder = new Builder();
            var node = builder.Build(nodeFactory, _form);

            return node;
        }

        #endregion

        #region Protected

        protected string GetSingleValue(IList<ICliCommandEntry> entries, string alias)
        {
            // todo: can throw
            return entries
                .Where(x => x is KeyValueCliCommandEntry)
                .Cast<KeyValueCliCommandEntry>()
                .Single(x => string.Equals(x.Alias, alias, StringComparison.InvariantCultureIgnoreCase)).Value;
        }

        #endregion

        #region ICliWorker Members

        public ICliAddIn AddIn { get; internal set; }

        public abstract void Process(IList<ICliCommandEntry> entries);

        #endregion

        #region ICliFunctionalityProvider Members

        public string Name { get; }

        public TextWriter Output
        {
            get => this.AddIn.Output;
            set => throw new NotSupportedException(); // todo: message 'use writer of owner'
        }

        public TextReader Input
        {
            get => this.AddIn.Input;
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
            return "todo: worker help.";
        }

        #endregion
    }
}
