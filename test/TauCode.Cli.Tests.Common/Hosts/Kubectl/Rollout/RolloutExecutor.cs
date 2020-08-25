using TauCode.Extensions;

namespace TauCode.Cli.Tests.Common.Hosts.Kubectl.Rollout
{
    public class RolloutExecutor : CommonExecutor
    {
        public RolloutExecutor()
            : base(
                typeof(RolloutExecutor).Assembly.GetResourceText(".Kubectl.Rollout.NoName.lisp", true),
                null,
                false)
        {
        }
    }
}
