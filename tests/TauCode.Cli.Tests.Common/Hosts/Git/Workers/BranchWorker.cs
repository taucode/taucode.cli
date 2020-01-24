using TauCode.Extensions;

namespace TauCode.Cli.Tests.Common.Hosts.Git.Workers
{
    public class BranchWorker : CommonWorker
    {
        public BranchWorker()
            : base(
                typeof(BranchWorker).Assembly.GetResourceText(".Git.NoName.Branch.lisp", true),
                null,
                true)
        {
        }
    }
}
