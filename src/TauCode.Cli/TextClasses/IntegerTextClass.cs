using TauCode.Parsing;

namespace TauCode.Cli.TextClasses
{
    [TextClass("integer-text")]
    public class IntegerTextClass : TextClassBase
    {
        public static readonly IntegerTextClass Instance = new IntegerTextClass();

        private IntegerTextClass()
        {
            
        }

        protected override string TryConvertFromImpl(string text, ITextClass anotherClass)
        {
            return null; // won't convert from anything.
        }
    }
}
