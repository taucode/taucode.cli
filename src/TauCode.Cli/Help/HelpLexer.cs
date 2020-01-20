using TauCode.Cli.Help.Producers;
using TauCode.Parsing.Lexing;

namespace TauCode.Cli.Help
{
    internal class HelpLexer : LexerBase
    {
        protected override ITokenProducer[] CreateProducers()
        {
            return new ITokenProducer[]
            {
                new WhiteSpaceTokenProducer(),
                new HelpTextTokenProducer(),
            };
        }
    }
}
