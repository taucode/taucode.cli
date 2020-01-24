using System.Collections.Generic;
using TauCode.Cli.Data;
using TauCode.Extensions;

namespace TauCode.Cli.Tests.Common.BadHosts
{
    public class WorkerWithNoNameButWithVersion : CliWorkerBase
    {
        public WorkerWithNoNameButWithVersion()
            : base(
                typeof(WorkerWithNoNameButWithVersion).Assembly.GetResourceText(".BadHostResources.UnnamedWorker.lisp", true),
                "has-version",
                false)
        {
        }

        public override void Process(IList<CliCommandEntry> entries)
        {
            // idle
        }
    }
}
