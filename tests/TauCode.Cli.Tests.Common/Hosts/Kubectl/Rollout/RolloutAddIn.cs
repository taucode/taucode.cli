using System.Collections.Generic;

namespace TauCode.Cli.Tests.Common.Hosts.Kubectl.Rollout
{
    public class RolloutAddIn : CliAddInBase
    {
        public RolloutAddIn()
            : base("rollout", null, true)
        {
        }

        protected override IReadOnlyList<ICliWorker> CreateWorkers()
        {
            return new ICliWorker[]
            {
                new RolloutWorker(),
            };
        }
    }
}
