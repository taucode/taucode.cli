using TauCode.Parsing.Tokens;

namespace TauCode.Cli.TextClasses
{
    public class PathTextClass : ITextClass
    {
        public static readonly PathTextClass Instance = new PathTextClass();

        private PathTextClass()
        {
        }
    }
}
