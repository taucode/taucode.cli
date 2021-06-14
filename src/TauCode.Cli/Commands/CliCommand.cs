using System;
using System.Collections.Generic;
using TauCode.Cli.Exceptions;

namespace TauCode.Cli.Commands
{
    public class CliCommand
    {
        private CliCommand(string addInName, string executorName)
        {
            this.AddInName = addInName;
            this.ExecutorName = executorName;
        }

        public string AddInName { get; }
        public string ExecutorName { get; private set; }
        public IList<CliCommandEntry> Entries { get; } = new List<CliCommandEntry>();

        public static CliCommand CreateAddInCommand(string addInName)
        {
            if (addInName == null)
            {
                throw new ArgumentNullException(nameof(addInName));
            }

            return new CliCommand(addInName, null);
        }

        public static CliCommand CreateExecutorCommand(string executorName)
        {
            if (executorName == null)
            {
                throw new ArgumentNullException(nameof(executorName));
            }

            return new CliCommand(null, executorName);
        }

        public static CliCommand CreateNamelessExecutorCommand()
        {
            return new CliCommand(null, null);
        }

        public void SetExecutorName(string executorName)
        {
            if (this.AddInName == null)
            {
                throw new CliException("Executor name can only be set if add-in name is not null.");
            }

            if (this.ExecutorName != null)
            {
                throw new CliException("Cannot change already existing executor name.");
            }

            this.ExecutorName = executorName ?? throw new ArgumentNullException(nameof(executorName));
        }
    }
}
