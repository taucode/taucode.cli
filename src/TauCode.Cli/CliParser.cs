//using System.Linq;
//using TauCode.Cli.Data;
//using TauCode.Parsing;
//using TauCode.Parsing.Building;
//using TauCode.Parsing.Lexing;
//using TauCode.Parsing.TinyLisp;

//namespace TauCode.Cli
//{
//    public class CliParser
//    {
//        private readonly IParser _parser;
//        private readonly INode _root;
//        private readonly ILexer _cliLexer;

//        public CliParser(string cliGrammar)
//        {
//            ILexer tinyLispLexer = new TinyLispLexer();
//            var tokens = tinyLispLexer.Lexize(cliGrammar);

//            var reader = new TinyLispPseudoReader();
//            var list = reader.Read(tokens);
//            IBuilder builder = new Builder();
//            INodeFactory cliNodeFactory = new CliNodeFactory("todo");

//            _root = builder.Build(cliNodeFactory, list);
//            _parser = new Parser();
//            _cliLexer = new CliLexer();
//        }

//        public CliCommand Parse(string commandText)
//        {
//            var tokens = _cliLexer.Lexize(commandText);
//            var res = _parser.Parse(_root, tokens);
//            return (CliCommand)res.Single();
//        }
//    }
//}
// todo remove