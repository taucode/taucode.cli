using System.Collections.Generic;

namespace TauCode.Cli.Tests.TestCli
{
    public class DbAddIn : CliAddInBase
    {
        public DbAddIn(ICliHost host)
            : base(
                host,
                "db",
                "sd-1.0",
                true)
        {
        }

        protected override IEnumerable<ICliWorker> CreateWorkers()
        {
            return new ICliWorker[]
            {
                new SdWorker(this),
            };
        }
    }
}
