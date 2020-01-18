using System.Collections.Generic;
using TauCode.Cli.Data;
using TauCode.Extensions;

namespace TauCode.Cli.Tests.Common.Hosts.Git.Workers
{
    public class BranchWorker : CliWorkerBase
    {
        public BranchWorker()
            : base(
                typeof(BranchWorker).Assembly.GetResourceText(".Git.NoName.Branch.lisp", true),
                null,
                true)
        {
        }

        public override void Process(IList<CliCommandEntry> entries)
        {
            this.Output.WriteLine("Dummy implementation. Get back here when ready.");
        }
    }
}
