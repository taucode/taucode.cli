using System.Collections.Generic;
using TauCode.Cli.Data;
using TauCode.Extensions;

namespace TauCode.Cli.Tests.Common.Hosts.Tau.WebApi.Workers
{
    public class ControllerWorker : CliWorkerBase
    {
        public ControllerWorker()
            : base(
                typeof(ControllerWorker).Assembly.GetResourceText(".Tau.WebApi.Controller.lisp", true),
                "cqrs-1.0",
                true)
        {
        }

        public override void Process(IList<CliCommandEntry> entries)
        {
            this.Output.WriteLine("Dummy implementation. Get back here when ready.");
        }
    }
}
