using System.Collections.Generic;

namespace TauCode.Cli.Tests.Common.Hosts.Kubectl.Scale
{
    public class ScaleAddIn : CliAddInBase
    {
        public ScaleAddIn()
            : base("scale", null, true)
        {
        }

        protected override IReadOnlyList<ICliWorker> CreateWorkers()
        {
            return new ICliWorker[]
            {
                new ScaleWorker(),
            };
        }
    }
}
