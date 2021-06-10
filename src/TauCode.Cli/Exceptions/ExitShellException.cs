using System;

namespace TauCode.Cli.Exceptions
{
    [Serializable]
    public class ExitShellException : CliException
    {
        public ExitShellException()
            : this("Shell exit requested.")
        {

        }

        public ExitShellException(string message)
            : base(message)
        {
        }

        public ExitShellException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
