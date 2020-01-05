using System.Collections.Generic;
using TauCode.Cli.Tests.TestCli.Workers;

namespace TauCode.Cli.Tests.TestCli.AddIns
{
    public class SqlAddIn : CliAddInBase
    {
        protected override IReadOnlyList<ICliWorker> CreateWorkers()
        {
            return new ICliWorker[]
            {
                new SqlWorker(),
            };
        }
    }
}
