using TauCode.Extensions;

namespace TauCode.Cli.Tests.Common.Hosts.Tau.Db.Executors
{
    public class MigrateExecutor : CommonExecutor
    {
        public MigrateExecutor()
            : base(
                typeof(DbAddIn).Assembly.GetResourceText("Migrate.lisp", true),
                null,
                true)
        {
        }
    }
}
