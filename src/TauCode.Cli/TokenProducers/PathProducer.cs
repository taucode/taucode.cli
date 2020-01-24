using TauCode.Cli.TextClasses;
using TauCode.Extensions;
using TauCode.Parsing;
using TauCode.Parsing.Lexing;
using TauCode.Parsing.TextDecorations;
using TauCode.Parsing.Tokens;

namespace TauCode.Cli.TokenProducers
{
    public class PathProducer : ITokenProducer
    {
        public LexingContext Context { get; set; }

        public IToken Produce()
        {
            var context = this.Context;
            var text = context.Text;
            var length = text.Length;

            var c = text[context.Index];

            if (IsPathFirstChar(c))
            {
                var initialIndex = context.Index;
                var index = initialIndex + 1;

                while (true)
                {
                    if (index == length)
                    {
                        break;
                    }

                    c = text[index];

                    if (IsPathInnerChar(c))
                    {
                        index++;
                        continue;
                    }

                    if (LexingHelper.IsInlineWhiteSpaceOrCaretControl(c))
                    {
                        break;
                    }

                    return null;
                }

                var delta = index - initialIndex;
                var str = text.Substring(initialIndex, delta);

                var position = new Position(context.Line, context.Column);
                context.Advance(delta, 0, context.Column + delta);

                var token = new TextToken(
                    PathTextClass.Instance,
                    NoneTextDecoration.Instance,
                    str,
                    position,
                    delta);

                return token;
            }

            return null;
        }

        public static bool IsPathFirstChar(char c) =>
            LexingHelper.IsDigit(c) ||
            LexingHelper.IsLatinLetter(c) ||
            c.IsIn('\\', '/', '.', '!', '~', '$', '%', ';');

        public static bool IsPathInnerChar(char c) =>
            IsPathFirstChar(c) || c.IsIn(':', '-');
    }
}
