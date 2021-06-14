using System.Collections.Generic;
using TauCode.Cli.Commands;
using TauCode.Extensions;

namespace TauCode.Cli.Tests.Common.BadHosts
{
    public class HostWithBadAddIn : CliHostBase
    {
        private class NamedExecutor : CliExecutorBase
        {
            public NamedExecutor()
                : base(
                    typeof(NamedExecutor).Assembly.GetResourceText(".BadHostResources.NamedExecutor.lisp", true), 
                    null,
                    false)
            {
            }

            public override void Process(IList<CliCommandEntry> entries)
            {
                // idle
            }
        }

        private class UnnamedExecutor : CliExecutorBase
        {
            public UnnamedExecutor()
                : base(
                    typeof(UnnamedExecutor).Assembly.GetResourceText(".BadHostResources.UnnamedExecutor.lisp", true),
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

            protected override IReadOnlyList<ICliExecutor> CreateExecutors()
            {
                return new List<ICliExecutor>
                {
                    new NamedExecutor(),
                    new UnnamedExecutor(),
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