using System;

namespace TauCode.Cli.Reading
{
    [Serializable]
    public class ReadException : Exception
    {
        public ReadException(string message)
            : base(message)
        {
        }

        public ReadException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
