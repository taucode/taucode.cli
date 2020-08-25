using System.Collections.Generic;
using TauCode.Cli.Data;
using TauCode.Extensions;

namespace TauCode.Cli.Tests.Common.BadHosts
{
    public class HostWithBadAddIn : CliHostBase
    {
        private class NamedWorker : CliExecutorBase
        {
            public NamedWorker()
                : base(
                    typeof(NamedWorker).Assembly.GetResourceText(".BadHostResources.NamedWorker.lisp", true), 
                    null,
                    false)
            {
            }

            public override void Process(IList<CliCommandEntry> entries)
            {
                // idle
            }
        }

        private class UnnamedWorker : CliExecutorBase
        {
            public UnnamedWorker()
                : base(
                    typeof(UnnamedWorker).Assembly.GetResourceText(".BadHostResources.UnnamedWorker.lisp", true),
                    null,
                    false)
            {
            }

            public override void Process(IList<CliCommandEntry> entries)
            {
                // idle
            }
        }


        private class BadAddIn : CliAddInBase
        {
            public BadAddIn()
                : base("bad", null, false)
            {
            }

            protected override IReadOnlyList<ICliExecutor> CreateWorkers()
            {
                return new List<ICliExecutor>
                {
                    new NamedWorker(),
                    new UnnamedWorker(),
                };
            }
        }

        public HostWithBadAddIn()
            : base("dummy", null, false)
        {
        }

        protected override IReadOnlyList<ICliAddIn> CreateAddIns()
        {
            return new List<ICliAddIn>
            {
                new BadAddIn(),
            };
        }
    }
}