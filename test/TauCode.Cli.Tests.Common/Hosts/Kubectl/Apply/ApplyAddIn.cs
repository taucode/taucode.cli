using System.Collections.Generic;

namespace TauCode.Cli.Tests.Common.Hosts.Kubectl.Apply
{
    public class ApplyAddIn : CliAddInBase
    {
        public ApplyAddIn()
            : base("apply", null, true)
        {
        }

        protected override IReadOnlyList<ICliExecutor> CreateExecutors()
        {
            return new ICliExecutor[]
            {
                new ApplyExecutor(),
            };
        }
    }
}
