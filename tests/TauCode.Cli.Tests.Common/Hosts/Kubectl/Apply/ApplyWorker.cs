using TauCode.Extensions;

namespace TauCode.Cli.Tests.Common.Hosts.Kubectl.Apply
{
    public class ApplyWorker : CommonWorker
    {
        public ApplyWorker()
            : base(
                typeof(ApplyWorker).Assembly.GetResourceText(".Kubectl.Apply.NoName.lisp", true),
                null,
                false)
        {
        }
    }
}
