using System.Collections.Generic;
using TauCode.Cli.Demo.Hosts.Tau.Db;
using TauCode.Cli.Demo.Hosts.Tau.WebApi;

namespace TauCode.Cli.Demo.Hosts.Tau
{
    /// <summary>
    /// Demo host for 'tau' command
    /// Has two add-ins: 'db' and 'web-api', each of which has two workers:
    /// 'db': 'sd' / 'serialize-data', 'dat' / 'drop-all-tables'
    /// 'web-api': 'cqrs', 'ctrl' / 'create-controller'
    /// </summary>
    public class TauHost : CliHostBase
    {
        public TauHost()
            : base("tau", "tau-1.0", true)
        {
        }

        protected override IReadOnlyList<ICliAddIn> CreateAddIns()
        {
            return new ICliAddIn[]
            {
                new DbAddIn(),
                new WebApiAddIn(),
            };
        }
    }
}
