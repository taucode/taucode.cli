using System;
using System.Collections.Generic;
using TauCode.Cli.Data;

namespace TauCode.Cli.Demo.Hosts.Tau.WebApi.Workers
{
    public class CqrsWorker : CliWorkerBase
    {
        public CqrsWorker()
            : base("todo", "todo", true)
        {
        }

        public override void Process(IList<ICliCommandEntry> entries)
        {
            throw new NotImplementedException();
        }
    }
}
