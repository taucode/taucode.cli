using TauCode.Extensions;

namespace TauCode.Cli.Tests.Common.Hosts.Tau.WebApi.Workers
{
    public class ControllerWorker : CommonWorker
    {
        public ControllerWorker()
            : base(
                typeof(ControllerWorker).Assembly.GetResourceText(".Tau.WebApi.Controller.lisp", true),
                "cqrs-1.0",
                true)
        {
        }
    }
}
