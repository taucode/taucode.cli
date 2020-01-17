using TauCode.Parsing;
using TauCode.Parsing.TextClasses;

namespace TauCode.Cli.TextClasses
{
    [TextClass("url")]
    public class UrlTextClass : TextClassBase
    {
        public static readonly UrlTextClass Instance = new UrlTextClass();

        private UrlTextClass()
        {
        }

        protected override string TryConvertFromImpl(string text, ITextClass anotherClass)
        {
            if (anotherClass is StringTextClass)
            {
                return text;
            }

            return null;
        }
    }
}
