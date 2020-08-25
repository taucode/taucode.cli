using TauCode.Extensions;

namespace TauCode.Cli.Tests.Common.Hosts.Tau.WebApi.Executors
{
    public class ControllerExecutor : CommonExecutor
    {
        public ControllerExecutor()
            : base(
                typeof(ControllerExecutor).Assembly.GetResourceText(".Tau.WebApi.Controller.lisp", true),
                "controller-1.0",
                true)
        {
        }
    }
}
