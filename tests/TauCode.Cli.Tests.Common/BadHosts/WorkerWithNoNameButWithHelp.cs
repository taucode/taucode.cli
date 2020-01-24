using System.Collections.Generic;
using TauCode.Cli.Data;
using TauCode.Extensions;

namespace TauCode.Cli.Tests.Common.BadHosts
{
    public class WorkerWithNoNameButWithHelp : CliWorkerBase
    {
        public WorkerWithNoNameButWithHelp()
            : base(
                typeof(WorkerWithNoNameButWithHelp).Assembly.GetResourceText(".BadHostResources.UnnamedWorker.lisp", true),
                null,
                true)
        {
        }

        public override void Process(IList<CliCommandEntry> entries)
        {
            // idle
        }
    }
}
