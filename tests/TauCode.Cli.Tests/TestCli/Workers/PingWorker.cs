using System;
using System.Collections.Generic;
using TauCode.Cli.Data;

namespace TauCode.Cli.Tests.TestCli.Workers
{
    public class PingWorker : CliWorkerBase
    {
        public PingWorker()
            : base("todo.", null, false)
        {
        }

        public override void Process(IList<ICliCommandEntry> entries)
        {
            throw new NotImplementedException();
        }
    }
}
