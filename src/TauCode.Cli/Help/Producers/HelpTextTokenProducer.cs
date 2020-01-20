using TauCode.Cli.Help.Tokens;
using TauCode.Parsing;
using TauCode.Parsing.Lexing;

namespace TauCode.Cli.Help.Producers
{
    internal class HelpTextTokenProducer : ITokenProducer
    {
        public LexingContext Context { get; set; }

        public IToken Produce()
        {
            var context = this.Context;
            var length = context.Length;
            var text = context.Text;
            var c = text[context.Index];
            
            if (LexingHelper.IsInlineWhiteSpaceOrCaretControl(c))
            {
                return null;
            }

            var initialIndex = this.Context.Index;
            var index = initialIndex;

            while (true)
            {
                if (index == length)
                {
                    break;
                }

                c = text[index];
                if (LexingHelper.IsInlineWhiteSpaceOrCaretControl(c))
                {
                    break;
                }

                index++;
            }

            var delta = index - initialIndex;
            var str = text.Substring(initialIndex, delta);
            var token = new HelpTextToken(str, new Position(context.Line, context.Column + delta), delta);
            this.Context.Advance(delta, 0, context.Column + delta);
            return token;
        }
    }
}
