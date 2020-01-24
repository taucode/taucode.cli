using System;

namespace TauCode.Cli.Exceptions
{
    [Serializable]
    public class CliCustomHandlerException : CliException
    {
        public CliCustomHandlerException()
            : base("Custom handler invoked.")
        {
        }
    }
}
