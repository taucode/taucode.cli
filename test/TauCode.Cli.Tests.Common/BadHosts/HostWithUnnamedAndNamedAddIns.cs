using System.Collections.Generic;
using TauCode.Cli.Commands;

namespace TauCode.Cli.Tests.Common.BadHosts
{
    public class HostWithUnnamedAndNamedAddIns : CliHostBase
    {
        private class ExecutorOne : CliExecutorBase
        {
            public ExecutorOne(string grammar, string version, bool supportsHelp) : base(grammar, version, supportsHelp)
            {
            }

            public override void Process(IList<CliCommandEntry> entries)
            {
                this.Output.WriteLine("Dummy implementation. Get back here when ready.");
            }
        }

        private class NamedAddIn : CliAddInBase
        {
            public NamedAddIn()
                : base("dummy-addin", null, false)
            {

            }

            protected override IReadOnlyList<ICliExecutor> CreateExecutors()
            {
                return null; // will throw; postponed.
            }
        }

        private class UnnamedAddIn : CliAddInBase
        {
            protected override IReadOnlyList<ICliExecutor> CreateExecutors()
            {
                return null; // will throw; postponed.
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
