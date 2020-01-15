using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TauCode.Cli.Data;
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

        #region Private

        private string ExtractName()
        {
            var topDefblock = _form.Single(x => x.GetSingleArgumentAsBool(":is-top") ?? false);
            var supposedCommandForm = topDefblock.GetFreeArguments().First();
            string name = null;
            if (supposedCommandForm.GetCarSymbolName() == "WORKER")
            {
                name = supposedCommandForm.GetSingleKeywordArgument<Symbol>(":worker-name", true)?.Name;
            }

            return name;
        }

        private INode BuildNode()
        {
            INodeFactory nodeFactory = new CliNodeFactory($"Todo: worker node factory. Name:'{this.Name}'");

            ITreeBuilder builder = new TreeBuilder();
            var node = builder.Build(nodeFactory, _form);

            return node;
        }

        #endregion

        #region Protected

        protected string GetSingleValue(IList<CliCommandEntry> entries, string alias)
        {
            throw new NotImplementedException();
            //var wantedEntries = entries
            //    .Where(x => x is KeyValueCliCommandEntry)
            //    .Cast<KeyValueCliCommandEntry>()
            //    .ToList();

            //if (wantedEntries.Count == 0)
            //{
            //    throw new CliException($"Entry with alias '{alias}' was not provided.");
            //}
            //else if (wantedEntries.Count == 1)
            //{
            //    return wantedEntries.Single().Value;
            //}
            //else
            //{
            //    throw new CliException($"Entry with alias '{alias}' was provided more than once.");
            //}
        }

        #endregion

        #region ICliWorker Members

        public ICliAddIn AddIn { get; internal set; }

        public abstract void Process(IList<CliCommandEntry> entries);

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

        public virtual string GetHelp()
        {
            return "todo: worker help.";
        }

        #endregion
    }
}
