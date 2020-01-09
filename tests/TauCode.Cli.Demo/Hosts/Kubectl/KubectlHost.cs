using System.Collections.Generic;
using TauCode.Cli.Demo.Hosts.Kubectl.Apply;
using TauCode.Cli.Demo.Hosts.Kubectl.Rollout;
using TauCode.Cli.Demo.Hosts.Kubectl.Scale;

namespace TauCode.Cli.Demo.Hosts.Kubectl
{
    public class KubectlHost : CliHostBase
    {
        public KubectlHost()
            : base("kubectl", "k8s-1.0", true)
        {
        }

        protected override IReadOnlyList<ICliAddIn> CreateAddIns()
        {
            return new ICliAddIn[]
            {
                new ApplyAddIn(),
                new RolloutAddIn(),
                new ScaleAddIn(),
            };
        }
    }
}
