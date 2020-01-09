using System.Collections.Generic;

namespace TauCode.Cli.Demo.Hosts.Kubectl.Apply
{
    public class ApplyAddIn : CliAddInBase
    {
        public ApplyAddIn()
            : base("apply", null, true)
        {
        }

        protected override IReadOnlyList<ICliWorker> CreateWorkers()
        {
            return new ICliWorker[]
            {
                new ApplyWorker(),
            };
        }
    }
}
