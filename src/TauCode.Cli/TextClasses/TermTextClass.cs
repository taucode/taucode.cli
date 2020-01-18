using TauCode.Parsing;

namespace TauCode.Cli.TextClasses
{
    [TextClass("term")]
    public class TermTextClass : TextClassBase
    {
        public static readonly TermTextClass Instance = new TermTextClass();

        private TermTextClass()
        {   
        }

        protected override string TryConvertFromImpl(string text, ITextClass anotherClass)
        {
            return null;
        }
    }
}
