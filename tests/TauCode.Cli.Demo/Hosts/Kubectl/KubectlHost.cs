using System;
using System.Collections.Generic;

namespace TauCode.Cli.Demo.Hosts.Kubectl
{
    public class KubectlHost : CliHostBase
    {
        public KubectlHost(string name, string version, bool supportsHelp) : base(name, version, supportsHelp)
        {
        }

        protected override IReadOnlyList<ICliAddIn> CreateAddIns()
        {
            throw new NotImplementedException();
        }
    }
}
