using System.Collections.Generic;
using TauCode.Cli.Data;
using TauCode.Extensions;

namespace TauCode.Cli.Demo.Hosts.Git.Workers
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

        public override void Process(IList<ICliCommandEntry> entries)
        {
            throw new System.NotImplementedException();
        }
    }
}
