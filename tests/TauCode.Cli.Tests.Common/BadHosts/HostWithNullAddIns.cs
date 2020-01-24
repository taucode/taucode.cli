using System.Collections.Generic;

namespace TauCode.Cli.Tests.Common.BadHosts
{
    public class HostWithNullAddIns : CliHostBase
    {
        public HostWithNullAddIns()
            : base("bad-host", null, false)
        {
        }

        protected override IReadOnlyList<ICliAddIn> CreateAddIns() => null;
    }
}
