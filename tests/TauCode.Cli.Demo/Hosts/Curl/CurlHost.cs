using System;
using System.Collections.Generic;

namespace TauCode.Cli.Demo.Hosts.Curl
{
    public class CurlHost : CliHostBase
    {
        public CurlHost(string name, string version, bool supportsHelp) : base(name, version, supportsHelp)
        {
        }

        protected override IReadOnlyList<ICliAddIn> CreateAddIns()
        {
            throw new NotImplementedException();
        }
    }
}
