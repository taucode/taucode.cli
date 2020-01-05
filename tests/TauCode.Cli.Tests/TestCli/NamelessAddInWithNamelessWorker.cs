using System;
using System.Collections.Generic;

namespace TauCode.Cli.Tests.TestCli
{
    public class NamelessAddInWithNamelessWorker : CliAddInBase
    {
        public NamelessAddInWithNamelessWorker()
        {
        }

        protected override IReadOnlyList<ICliWorker> CreateWorkers()
        {
            throw new NotImplementedException();
        }
    }
}
