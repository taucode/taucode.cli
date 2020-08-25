using System.Collections.Generic;
using TauCode.Cli.Tests.Common.Hosts.Git.Executors;

namespace TauCode.Cli.Tests.Common.Hosts.Git
{
    /// <summary>
    /// Nameless add-in
    /// </summary>
    public class GitAddIn : CliAddInBase
    {
        public GitAddIn()
            : base(null, null, false)
        {
        }

        protected override void OnNodeCreated()
        {   
        }

        protected override IReadOnlyList<ICliExecutor> CreateExecutors()
        {
            return new ICliExecutor[]
            {
                new BranchExecutor(),
                new CheckoutExecutor(),
            };
        }
    }
}
