using System;

namespace TauCode.Cli.Exceptions
{
    [Serializable]
    public class StopParsingException : Exception // todo: CliExceptionBase?
    {
        public StopParsingException()
        {
        }

        public StopParsingException(string message) : base(message)
        {
        }

        public StopParsingException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
