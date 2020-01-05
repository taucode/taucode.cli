using System.Collections.Generic;
using TauCode.Cli.Tests.TestCli.Workers;

namespace TauCode.Cli.Tests.TestCli.AddIns
{
    public class DbAddIn : CliAddInBase
    {
        public DbAddIn()
            : base(
                "db",
                "db-1.0",
                true)
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
