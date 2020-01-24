using TauCode.Extensions;

namespace TauCode.Cli.Tests.Common.Hosts.Kubectl.Rollout
{
    public class RolloutWorker : CommonWorker
    {
        public RolloutWorker()
            : base(
                typeof(RolloutWorker).Assembly.GetResourceText(".Kubectl.Rollout.NoName.lisp", true),
                null,
                false)
        {
        }
    }
}
