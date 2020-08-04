using System.Collections.Generic;
using TauCode.Cli.Tests.Common.Hosts.Tau.Db;

namespace TauCode.Cli.Tests.Common.Hosts.Tau
{
    public class TauHost : CliHostBase
    {
        public TauHost()
            : base("tau", "tau-1.0", true)
        {
        }

        public TauHost(string version, bool supportsHelp)
            : base("tau", version, supportsHelp)
        {

        }

        protected override IReadOnlyList<ICliAddIn> CreateAddIns()
        {
            return new List<ICliAddIn>
            {
                new DbAddIn(),
            };
        }
    }
}
