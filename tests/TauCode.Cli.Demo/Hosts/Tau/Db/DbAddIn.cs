using System.Collections.Generic;
using TauCode.Cli.Demo.Hosts.Tau.Db.Workers;

namespace TauCode.Cli.Demo.Hosts.Tau.Db
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
