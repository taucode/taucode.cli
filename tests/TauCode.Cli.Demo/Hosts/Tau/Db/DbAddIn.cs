using System;
using System.Collections.Generic;

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
            throw new NotImplementedException();
        }
    }
}
