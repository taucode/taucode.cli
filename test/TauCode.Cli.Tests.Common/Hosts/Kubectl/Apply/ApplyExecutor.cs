using TauCode.Extensions;

namespace TauCode.Cli.Tests.Common.Hosts.Kubectl.Apply
{
    public class ApplyExecutor : CommonExecutor
    {
        public ApplyExecutor()
            : base(
                typeof(ApplyExecutor).Assembly.GetResourceText(".Kubectl.Apply.NoName.lisp", true),
                null,
                false)
        {
        }
    }
}
