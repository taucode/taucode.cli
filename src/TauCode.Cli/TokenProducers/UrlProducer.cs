using TauCode.Cli.TextClasses;
using TauCode.Parsing;
using TauCode.Parsing.Lab.Lexing;
using TauCode.Parsing.Lexing;
using TauCode.Parsing.TextDecorations;
using TauCode.Parsing.Tokens;

namespace TauCode.Cli.TokenProducers
{
    public class UrlProducer : ITokenProducer
    {
        private static readonly string[] PossibleStarts =
        {
            "http://",
            "https://"
        };

        public LexingContext Context { get; set; }

        public IToken Produce()
        {
            var context = this.Context;
            var text = context.Text;
            var length = text.Length;

            string start = null;
            foreach (var possibleStart in PossibleStarts)
            {
                if (context.StartsWith(possibleStart))
                {
                    start = possibleStart;
                    break;
                }
            }

            if (start == null)
            {
                return null;
            }

            var initialIndex = context.Index;
            var startIndex = initialIndex + start.Length;
            var index = startIndex;

            while (true)
            {
                if (index == length)
                {
                    break;
                }

                var c = text[index];

                if (LexingHelper.IsInlineWhiteSpaceOrCaretControl(c))
                {
                    break;
                }

                index++;
            }

            if (index == startIndex)
            {
                return null;
            }

            var delta = index - initialIndex;
            var str = text.Substring(initialIndex, delta);

            var position = new Position(context.Line, context.Column);
            context.Advance(delta, 0, context.Column + delta);

            var token = new TextToken(
                UrlTextClass.Instance,
                NoneTextDecoration.Instance,
                str,
                position,
                delta);

            return token;
        }
    }
}
