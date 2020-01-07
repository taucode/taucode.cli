using System;
using System.Collections.Generic;

namespace TauCode.Cli.Demo.Hosts.Git
{
    public class GitHost : CliHostBase
    {
        public GitHost(string name, string version, bool supportsHelp) : base(name, version, supportsHelp)
        {
        }

        protected override IReadOnlyList<ICliAddIn> CreateAddIns()
        {
            throw new NotImplementedException();
        }
    }
}
