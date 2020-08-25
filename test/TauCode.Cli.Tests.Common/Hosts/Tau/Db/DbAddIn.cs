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
            this.Description = "Database-related operations";
        }

        protected override IReadOnlyList<ICliExecutor> CreateWorkers()
        {
            return new List<ICliExecutor>
            {
                new ClearAllTablesWorker(),
                new ConvertMetadataWorker(),
                new DropAllTablesWorker(),
                new SerializeDataWorker(),
                new SerializeMetadataWorker(),
                new MigrateWorker(),
            };
        }
    }
}
