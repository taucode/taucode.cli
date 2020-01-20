using TauCode.Extensions;

namespace TauCode.Cli.Tests.Common.Hosts.Kubectl.Scale
{
    public class ScaleWorker : CommonWorker
    {
        public ScaleWorker()
            : base(
                typeof(ScaleWorker).Assembly.GetResourceText(".Kubectl.Scale.NoName.lisp", true),
                null,
                false)
        {
        }
    }
}
