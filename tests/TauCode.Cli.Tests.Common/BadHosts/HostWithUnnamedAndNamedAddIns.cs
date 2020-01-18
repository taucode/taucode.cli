using System.Collections.Generic;
using TauCode.Cli.Data;

namespace TauCode.Cli.Tests.Common.BadHosts
{
    // todo finish this job
    public class HostWithUnnamedAndNamedAddIns : CliHostBase
    {
        private class WorkerOne : CliWorkerBase
        {
            public WorkerOne(string grammar, string version, bool supportsHelp) : base(grammar, version, supportsHelp)
            {
            }

            public override void Process(IList<CliCommandEntry> entries)
            {
                throw new System.NotImplementedException();
            }
        }

        private class NamedAddIn : CliAddInBase
        {
            public NamedAddIn()
                : base("dummy-addin", null, false)
            {

            }

            protected override IReadOnlyList<ICliWorker> CreateWorkers()
            {
                throw new System.NotImplementedException();
            }
        }

        private class UnnamedAddIn : CliAddInBase
        {
            protected override IReadOnlyList<ICliWorker> CreateWorkers()
            {
                throw new System.NotImplementedException();
            }
        }

        public HostWithUnnamedAndNamedAddIns()
            : base("bad-host", null, false)
        {
        }

        protected override IReadOnlyList<ICliAddIn> CreateAddIns()
        {
            return new ICliAddIn[]
            {
                new NamedAddIn(),
                new UnnamedAddIn(),
            };
        }
    }
}
