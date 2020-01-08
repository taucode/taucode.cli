using System.Collections.Generic;
using TauCode.Cli.Demo.Hosts.Git.Workers;

namespace TauCode.Cli.Demo.Hosts.Git
{
    /// <summary>
    /// Nameless add-in
    /// </summary>
    public class GitAddIn : CliAddInBase
    {
        protected override IReadOnlyList<ICliWorker> CreateWorkers()
        {
            return new ICliWorker[]
            {
                new BranchWorker(),
                new CheckoutWorker(),
                new CloneWorker(),
            };
        }
    }
}
