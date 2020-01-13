using TauCode.Parsing;

namespace TauCode.Cli.TextClasses
{
    public class PathTextClass : TextClassBase
    {
        public static readonly PathTextClass Instance = new PathTextClass();

        private PathTextClass()
        {
        }

        protected override string TryConvertFromImpl(string text, ITextClass anotherClass)
        {
            throw new System.NotImplementedException();
        }
    }
}
