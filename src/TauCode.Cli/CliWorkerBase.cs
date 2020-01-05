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
    // todo clean up
    public abstract class CliWorkerBase : ICliWorker
    {
        #region Fields

        //private readonly string _grammar;
        private INode _node;
        private readonly PseudoList _form;

        #endregion

        #region Constructor

        protected CliWorkerBase(
            ICliAddIn addIn,
            string grammar,
            string version,
            bool supportsHelp)
        {
            this.AddIn = addIn ?? throw new ArgumentNullException(nameof(addIn));
            //_grammar = grammar ?? throw new ArgumentNullException(nameof(grammar));

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

        #endregion

        #region Protected

        private INode BuildNode() // todo: need 'protected virtual'?
        {
            INodeFactory nodeFactory = new CliNodeFactory($"Todo: worker for alias '{this.Name}'");
            IBuilder builder = new Builder();
            var node = builder.Build(nodeFactory, _form);

            return node;
        }

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

        public ICliAddIn AddIn { get; }

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
            return "todo: worker help.";
        }

        #endregion

        #region Todo Old Stuff

        //protected CliWorkerBase(ICliAddIn addIn, string grammar)
        //{
        //    // todo checks

        //    this.AddIn = addIn;
        //    this.BuildNode(grammar, out var alias, out var node);
        //    this.Alias = alias;
        //    this.Node = node;
        //}

        //private void BuildNode(string grammar, out string alias, out INode node)
        //{
        //    var tinyLispLexer = new TinyLispLexer();
        //    var tinyLispPseudoReader = new TinyLispPseudoReader();
        //    var lispTokens = tinyLispLexer.Lexize(grammar);
        //    var form = tinyLispPseudoReader.Read(lispTokens);

        //    // todo: next lines can throw. use try/catch
        //    var topDefblock = form.Single(x => x.GetSingleArgumentAsBool(":is-top") ?? false);
        //    var supposedCommandForm = topDefblock.GetFreeArguments().First();
        //    alias = null;
        //    if (supposedCommandForm.GetCarSymbolName() == "WORKER")
        //    {
        //        alias = supposedCommandForm.GetSingleKeywordArgument<Symbol>(":alias").Name;
        //    }

        //    INodeFactory nodeFactory = new CliNodeFactory($"Todo: worker for alias '{alias}'");
        //    IBuilder builder = new Builder();
        //    node = builder.Build(nodeFactory, form);
        //}


        //public ICliAddIn AddIn { get; }

        //public string Alias { get; }

        //public INode Node { get; }

        //public abstract void Process(IList<ICliCommandEntry> entries);

        //public string GetHelp()
        //{
        //    throw new System.NotImplementedException();
        //}

        #endregion
    }
}
