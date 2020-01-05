using System;
using System.Collections.Generic;
using System.IO;
using TauCode.Cli.Data;
using TauCode.Parsing;

namespace TauCode.Cli
{
    // todo clean up
    public abstract class CliWorkerBase : ICliWorker
    {
        #region Fields

        private readonly string _grammar;
        private INode _node;

        #endregion

        #region Constructor

        protected CliWorkerBase(
            ICliAddIn addIn,
            string grammar,
            string version,
            bool supportsHelp)
        {
            this.AddIn = addIn ?? throw new ArgumentNullException(nameof(addIn));
            _grammar = grammar ?? throw new ArgumentNullException(nameof(grammar));
            this.Name = this.ExtractNameFromGrammar(grammar);
            this.Version = version;
            this.SupportsHelp = supportsHelp;
        }


        #endregion

        #region Private

        private string ExtractNameFromGrammar(string grammar)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Protected

        protected virtual INode BuildNode()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region ICliWorker Members

        public ICliAddIn AddIn { get; }

        public abstract void Process(IList<ICliCommandEntry> entries);

        #endregion

        #region ICliFunctionalityProvider Members

        public string Name { get; }
        public TextWriter Output => this.AddIn.Output;
        public TextReader Input => this.AddIn.Input;
        public INode Node => _node ?? (_node = this.BuildNode());
        public string Version { get; }
        public bool SupportsHelp { get; }

        public string GetHelp()
        {
            throw new NotImplementedException();
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

        //protected string GetSingleValue(IList<ICliCommandEntry> entries, string alias)
        //{
        //    // todo: can throw
        //    return entries
        //        .Where(x => x is KeyValueCliCommandEntry)
        //        .Cast<KeyValueCliCommandEntry>()
        //        .Single(x => string.Equals(x.Alias, alias, StringComparison.InvariantCultureIgnoreCase)).Value;
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
