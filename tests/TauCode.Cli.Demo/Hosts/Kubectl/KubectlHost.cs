using System;
using System.Collections.Generic;

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
            throw new NotImplementedException();
        }
    }
}
