using System.Collections.Generic;
using TauCode.Cli.Data;

namespace TauCode.Cli.Demo.Hosts.Git.Workers
{
    public class BranchWorker : CliWorkerBase
    {
        public BranchWorker()
            : base("todo", null, true)
        {
        }

        public override void Process(IList<ICliCommandEntry> entries)
        {
            throw new System.NotImplementedException();
        }
    }
}
