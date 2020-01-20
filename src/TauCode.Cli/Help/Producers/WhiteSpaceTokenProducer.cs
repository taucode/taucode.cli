using TauCode.Cli.Help.Tokens;
using TauCode.Parsing;
using TauCode.Parsing.Lexing;

namespace TauCode.Cli.Help.Producers
{
    public class WhiteSpaceTokenProducer : ITokenProducer
    {
        public LexingContext Context { get; set; }

        public IToken Produce()
        {
            var initialIndex = this.Context.Index;
            var text = this.Context.Text;
            var length = text.Length;
            var currentIndex = initialIndex;

            var initialLine = Context.Line;
            var lineShift = 0;
            var column = this.Context.Column;

            var c = text[initialIndex];
            if (!LexingHelper.IsInlineWhiteSpaceOrCaretControl(c))
            {
                return null;
            }

            while (true)
            {
                if (currentIndex == length)
                {
                    this.Context.Advance(currentIndex - initialIndex, lineShift, column);
                    return null;
                }

                c = text[currentIndex];
                switch (c)
                {
                    case '\t':
                    case ' ':
                        currentIndex++;
                        column++;
                        break;

                    case LexingHelper.CR:
                        currentIndex++;
                        lineShift++;
                        column = 0;

                        if (currentIndex < length)
                        {
                            var nextChar = text[currentIndex];
                            if (nextChar == LexingHelper.LF)
                            {
                                currentIndex++;
                            }
                        }

                        break;

                    case LexingHelper.LF:
                        currentIndex++;
                        lineShift++;
                        column = 0;
                        break;

                    default:
                        var delta = currentIndex - initialIndex;
                        var line = initialLine + lineShift;

                        var token = new WhiteSpaceToken(new Position(line, column), delta);
                        if (currentIndex > initialIndex)
                        {
                            this.Context.Advance(delta, lineShift, column);
                        }

                        return token;
                }
            }
        }
    }
}
