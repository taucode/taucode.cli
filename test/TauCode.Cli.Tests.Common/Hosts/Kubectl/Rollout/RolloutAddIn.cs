using System.Collections.Generic;

namespace TauCode.Cli.Tests.Common.Hosts.Kubectl.Rollout
{
    public class RolloutAddIn : CliAddInBase
    {
        public RolloutAddIn()
            : base("rollout", null, true)
        {
        }

        protected override IReadOnlyList<ICliExecutor> CreateExecutors()
        {
            return new ICliExecutor[]
            {
                new RolloutWorker(),
            };
        }
    }
}
