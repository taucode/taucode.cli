using System;
using TauCode.Cli.Data;

namespace TauCode.Cli.Exceptions
{
    [Serializable]
    public class CliCustomHandlerException : Exception
    {
        public CliCustomHandlerException(string addInName, string processorAlias, string tokenText)
        {
            // todo checks
            this.Command = command;
        }

        public CliCommand Command { get; }
    }
}
