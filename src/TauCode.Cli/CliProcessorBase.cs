using System.Collections.Generic;
using System.Linq;
using TauCode.Cli.Data;
using TauCode.Parsing;
using TauCode.Parsing.Building;
using TauCode.Parsing.TinyLisp;
using TauCode.Parsing.TinyLisp.Data;

namespace TauCode.Cli
{
    // todo clean up
    public abstract class CliProcessorBase : ICliProcessor
    {
        //private readonly string _grammar;
        //private readonly INode _node;

        protected CliProcessorBase(ICliAddIn addIn, string grammar)
        {
            // todo checks

            this.AddIn = addIn;
            this.BuildNode(grammar, out var alias, out var node);
            //this.Verb = verb;

            this.Alias = alias;
            this.Node = node;
        }

        //public string GetGrammar() => _grammar;

        private void BuildNode(string grammar, out string alias, out INode node)
        {
            var tinyLispLexer = new TinyLispLexer();
            var tinyLispPseudoReader = new TinyLispPseudoReader();
            var lispTokens = tinyLispLexer.Lexize(grammar);
            var form = tinyLispPseudoReader.Read(lispTokens);

            // todo: next lines can throw. use try/catch
            var topDefblock = form.Single(x => x.GetSingleArgumentAsBool(":is-top") ?? false);
            var supposedCommandForm = topDefblock.GetFreeArguments().First();
            alias = null;
            if (supposedCommandForm.GetCarSymbolName() == "PROCESSOR")
            {
                alias = supposedCommandForm.GetSingleKeywordArgument<Symbol>(":alias").Name;
            }

            INodeFactory nodeFactory = new CliNodeFactory($"Todo: processor for alias '{alias}'");
            IBuilder builder = new Builder();
            node = builder.Build(nodeFactory, form);


            //return processorNode;

        }

        //public string Verb { get; }
        //public INode Node { get; }

        //public INode BuildNode()
        //{
        //    throw new NotImplementedException();
        //}

        public ICliAddIn AddIn { get; }
        public string Alias { get; }
        public INode Node { get; }

        //public bool AcceptsCommand(CliCommand command)
        //{
        //    throw new System.NotImplementedException();
        //}

        //public void ProcessCommand(CliCommand command)
        //{
        //    throw new System.NotImplementedException();
        //}

        public abstract void Process(IList<ICliCommandEntry> entries);
        //{
        //}

        public string GetHelp()
        {
            throw new System.NotImplementedException();
        }
    }
}
