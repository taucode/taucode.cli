using System;
using System.Collections.Generic;
using TauCode.Cli.Data;

namespace TauCode.Cli.Demo.Hosts.Kubectl.Rollout
{
    public class RolloutWorker : CliWorkerBase
    {
        public RolloutWorker()
            : base("todo", null, true)
        {
        }

        public override void Process(IList<ICliCommandEntry> entries)
        {
            throw new NotImplementedException();
        }
    }
}
