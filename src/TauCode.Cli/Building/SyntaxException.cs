using System;

namespace TauCode.Cli.Building
{
    [Serializable]
    public class SyntaxException : Exception
    {
        public SyntaxException(string message)
            : base(message)
        {
        }

        public SyntaxException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}