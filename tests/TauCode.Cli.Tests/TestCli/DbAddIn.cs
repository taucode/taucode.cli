using System.Collections.Generic;

namespace TauCode.Cli.Tests.TestCli
{
    public class DbAddIn : CliAddInBase
    {
        public DbAddIn()
            : base(
                "db",
                "sd-1.0",
                true)
        {
        }

        protected override IReadOnlyList<ICliWorker> CreateWorkers()
        {
            return new ICliWorker[]
            {
                new SdWorker(),
            };
        }
    }
}
