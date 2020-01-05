using System.Collections.Generic;
using TauCode.Cli.Demo.AddIns.DbAddInWorkers;

namespace TauCode.Cli.Demo.AddIns
{
    public class DbAddIn : CliAddInBase
    {
        public DbAddIn()
            : base("db", "db-1488", true)//todo
        {
        }

        protected override IReadOnlyList<ICliWorker> CreateWorkers()
        {
            return new ICliWorker[]
            {       
                new SerializeDataWorker(),
            };
        }
    }
}
