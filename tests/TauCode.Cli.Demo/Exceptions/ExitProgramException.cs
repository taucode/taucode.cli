using System;

namespace TauCode.Cli.Demo.Exceptions
{
    [Serializable]
    public class ExitProgramException : Exception
    {

        public ExitProgramException()
        {
        }

        public ExitProgramException(string message)
            : base(message)
        {
        }

        public ExitProgramException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
