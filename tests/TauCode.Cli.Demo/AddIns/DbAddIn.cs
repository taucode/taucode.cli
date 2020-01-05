using System.Collections.Generic;
using TauCode.Cli.Demo.AddIns.DbAddInWorkers;

namespace TauCode.Cli.Demo.AddIns
{
    public class DbAddIn : CliAddInBase
    {
        public DbAddIn(ICliHost host)
            : base(host, "db", "db-1488", true)
        {
        }

        protected override IEnumerable<ICliWorker> CreateWorkers()
        {
            return new ICliWorker[]
            {       
                new SerializeDataWorker(this),
            };
        }
    }
}
