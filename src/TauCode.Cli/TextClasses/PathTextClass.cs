using TauCode.Parsing;
using TauCode.Parsing.TextClasses;

namespace TauCode.Cli.TextClasses
{
    [TextClass("path")]
    public class PathTextClass : TextClassBase
    {
        public static readonly PathTextClass Instance = new PathTextClass();

        private PathTextClass()
        {
        }

        protected override string TryConvertFromImpl(string text, ITextClass anotherClass)
        {
            if (
                anotherClass is StringTextClass ||
                anotherClass is TermTextClass ||
                anotherClass is KeyTextClass)
            {
                return text;
            }

            return null;
        }
    }
}
