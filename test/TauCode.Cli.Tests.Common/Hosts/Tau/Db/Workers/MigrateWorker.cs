using TauCode.Extensions;

namespace TauCode.Cli.Tests.Common.Hosts.Tau.Db.Workers
{
    public class MigrateWorker : CommonExecutor
    {
        public MigrateWorker()
            : base(
                typeof(DbAddIn).Assembly.GetResourceText("Migrate.lisp", true),
                null,
                true)
        {
        }
    }
}
