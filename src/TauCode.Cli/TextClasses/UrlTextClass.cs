using TauCode.Parsing;

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
            throw new System.NotImplementedException();
        }
    }
}