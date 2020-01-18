using System;
using System.Collections.Generic;

namespace TauCode.Cli.Data
{
    public class CliCommand
    {
        private CliCommand(string addInName, string workerName)
        {
            this.AddInName = addInName;
            this.WorkerName = workerName;
        }

        public string AddInName { get; }
        public string WorkerName { get; private set; }
        public IList<CliCommandEntry> Entries { get; } = new List<CliCommandEntry>();

        public static CliCommand CreateAddInCommand(string addInName)
        {
            if (addInName == null)
            {
                throw new ArgumentNullException(nameof(addInName));
            }

            return new CliCommand(addInName, null);
        }

        public static CliCommand CreateWorkerCommand(string workerName)
        {
            if (workerName == null)
            {
                throw new ArgumentNullException(nameof(workerName));
            }

            return new CliCommand(null, workerName);
        }

        public static CliCommand CreateNamelessWorkerCommand()
        {
            return new CliCommand(null, null);
        }

        public void SetWorkerName(string workerName)
        {
            if (this.AddInName == null)
            {
                throw new NotImplementedException(); // shouldn't be
            }

            if (this.WorkerName != null)
            {
                throw new NotImplementedException(); // shouldn't be
            }

            this.WorkerName = workerName ?? throw new ArgumentNullException(nameof(workerName));
        }
    }
}
