using System.Collections.Generic;
using TauCode.Cli.Tests.Common.Hosts.Tau.Db.Workers;

namespace TauCode.Cli.Tests.Common.Hosts.Tau.Db
{
    public class DbAddIn : CliAddInBase
    {
        public const string DefaultVersion = "db-1.0";
        public static string CurrentVersion { get; set; } = DefaultVersion;

        public DbAddIn()
            : base("db", CurrentVersion, true)
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
