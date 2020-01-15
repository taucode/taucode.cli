using System.Collections.Generic;
using TauCode.Cli.Tests.Common.Hosts.Tau.Db.Workers;

namespace TauCode.Cli.Tests.Common.Hosts.Tau.Db
{
    public class DbAddIn : CliAddInBase
    {
        public DbAddIn()
            : base("db", "db-1.0", true)
        {
        }

        protected override IReadOnlyList<ICliWorker> CreateWorkers()
        {
            return new ICliWorker[]
            {
                new SerializeDataWorker(),
                new DropAllTablesWorker(),
            };
        }
    }
}
