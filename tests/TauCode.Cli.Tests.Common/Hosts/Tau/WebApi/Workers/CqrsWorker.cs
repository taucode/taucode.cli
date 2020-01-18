using System.Collections.Generic;
using TauCode.Cli.Data;
using TauCode.Extensions;

namespace TauCode.Cli.Tests.Common.Hosts.Tau.WebApi.Workers
{
    public class CqrsWorker : CliWorkerBase
    {
        public CqrsWorker()
            : base(
                typeof(CqrsWorker).Assembly.GetResourceText(".Tau.WebApi.Cqrs.lisp", true),
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
