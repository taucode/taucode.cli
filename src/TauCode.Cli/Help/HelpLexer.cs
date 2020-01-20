using TauCode.Parsing.Lexing;
using TauCode.Parsing.Lexing.StandardProducers;

namespace TauCode.Cli.Help
{
    internal class HelpLexer : LexerBase
    {
        public HelpLexer()
            : base(false)
        {

        }

        protected override ITokenProducer[] CreateProducers()
        {
            return new ITokenProducer[]
            {
                new WhiteSpaceTokenProducer(),
                new CharSequenceProducer(),
            };
        }
    }
}
