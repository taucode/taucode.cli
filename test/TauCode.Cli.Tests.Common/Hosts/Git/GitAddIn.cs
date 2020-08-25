using System.Collections.Generic;
using TauCode.Cli.Tests.Common.Hosts.Git.Workers;

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
                new BranchWorker(),
                new CheckoutWorker(),
                //new CloneWorker(),
            };
        }
    }
}
