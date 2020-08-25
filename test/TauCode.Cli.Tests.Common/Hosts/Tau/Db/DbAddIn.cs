using System.Collections.Generic;
using TauCode.Cli.Tests.Common.Hosts.Tau.Db.Executors;

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

        protected override IReadOnlyList<ICliExecutor> CreateExecutors()
        {
            return new List<ICliExecutor>
            {
                new ClearAllTablesExecutor(),
                new ConvertMetadataExecutor(),
                new DropAllTablesExecutor(),
                new SerializeDataExecutor(),
                new SerializeMetadataExecutor(),
                new MigrateExecutor(),
            };
        }
    }
}
