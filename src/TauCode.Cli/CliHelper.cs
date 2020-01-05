using TauCode.Parsing.Tokens;

namespace TauCode.Cli
{
    public static class CliHelper
    {
        public static string GetTextTokenRepresentation(TextToken textToken)
        {
            return textToken.Text; // todo: need this at all?
        }
    }
}
