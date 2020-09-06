using System.Collections.Generic;
using TauCode.Cli.Tests.Common.Hosts.Work.Mock;

namespace TauCode.Cli.Tests.Common.Hosts.Work
{
    public class WorkerHost : CliHostBase
    {
        public WorkerHost(IBus bus)
            : base("work", "1.0", true)
        {
            this.Bus = bus;
        }

        public IBus Bus { get; }

        protected override IReadOnlyList<ICliAddIn> CreateAddIns()
        {
            return new ICliAddIn[]
            {
                new WorkerAddIn(),
            };
        }
    }
}
