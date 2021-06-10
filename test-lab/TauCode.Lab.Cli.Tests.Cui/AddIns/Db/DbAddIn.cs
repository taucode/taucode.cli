using System.Collections.Generic;
using TauCode.Cli;
using TauCode.Db.SqlClient;
using TauCode.Lab.Cli.Tests.Cui.AddIns.Db.Executors;

namespace TauCode.Lab.Cli.Tests.Cui.AddIns.Db
{
    public class DbAddIn : CliAddInBase
    {
        public DbAddIn()
            : base("db", "1.0", true)
        {

        }

        protected override IReadOnlyList<ICliExecutor> CreateExecutors()
        {
            return new List<ICliExecutor>
            {
                new FluentMigrateExecutor(xx => SqlUtilityFactory.Instance, this.GetType().Assembly), // todo
            };
        }
    }
}
