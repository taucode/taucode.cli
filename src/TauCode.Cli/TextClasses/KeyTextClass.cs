using TauCode.Parsing.Tokens;

namespace TauCode.Cli.TextClasses
{
    public class KeyTextClass : ITextClass
    {
        public static readonly KeyTextClass Instance = new KeyTextClass();

        private KeyTextClass()
        {
        }
    }
}