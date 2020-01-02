using TauCode.Parsing.Tokens;

namespace TauCode.Cli.TextClasses
{
    public class TermTextClass : ITextClass
    {
        public static readonly TermTextClass Instance = new TermTextClass();

        private TermTextClass()
        {   
        }
    }
}
