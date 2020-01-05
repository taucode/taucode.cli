using System;

namespace TauCode.Cli.Exceptions
{
    [Serializable]
    public class CliCustomHandlerException : Exception
    {
        public CliCustomHandlerException(
            string addInName,
            string workerAlias,
            string tokenText)
        {
            // todo checks
            this.AddInName = addInName;
            this.WorkerAlias = workerAlias;
            this.TokenText = tokenText;
        }

        public string AddInName { get; }

        public string WorkerAlias { get; }

        public string TokenText { get; }
    }
}
