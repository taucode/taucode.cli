using System.Collections.Generic;

namespace TauCode.Cli.Tests.Common.BadHosts
{
    public class HostWithoutAddIns : CliHostBase
    {
        public HostWithoutAddIns()
            : base("bad-host", null, false)
        {
        }

        protected override IReadOnlyList<ICliAddIn> CreateAddIns()
        {
            return new List<ICliAddIn>();
        }
    }
}
