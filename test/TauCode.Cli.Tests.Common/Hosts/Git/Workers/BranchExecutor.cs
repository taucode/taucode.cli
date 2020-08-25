using TauCode.Extensions;

namespace TauCode.Cli.Tests.Common.Hosts.Git.Workers
{
    public class BranchExecutor : CommonExecutor
    {
        public BranchExecutor()
            : base(
                typeof(BranchExecutor).Assembly.GetResourceText(".Git.NoName.Branch.lisp", true),
                null,
                true)
        {
        }
    }
}
