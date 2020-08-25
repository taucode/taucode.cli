using System.Collections.Generic;

namespace TauCode.Cli.Tests.Common.BadHosts
{
    public class HostWithEmptyAddIn : CliHostBase
    {
        private class EmptyAddIn : CliAddInBase
        {
            protected override IReadOnlyList<ICliExecutor> CreateExecutors()
            {
                return new List<ICliExecutor>();
            }
        }

        public HostWithEmptyAddIn()
            : base("dummy", null, false)
        {
        }

        protected override IReadOnlyList<ICliAddIn> CreateAddIns()
        {
            return new ICliAddIn[]
            {
                new EmptyAddIn(),
            };
        }
    }
}
