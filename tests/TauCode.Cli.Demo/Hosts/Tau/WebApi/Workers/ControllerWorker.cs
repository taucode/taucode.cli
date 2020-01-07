using System;
using System.Collections.Generic;
using TauCode.Cli.Data;

namespace TauCode.Cli.Demo.Hosts.Tau.WebApi.Workers
{
    public class ControllerWorker : CliWorkerBase
    {
        public ControllerWorker()
            : base("todo", "todo", true)
        {
        }

        public override void Process(IList<ICliCommandEntry> entries)
        {
            throw new NotImplementedException();
        }
    }
}
