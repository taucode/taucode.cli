using TauCode.Extensions;

namespace TauCode.Cli.Tests.Common.Hosts.Tau.WebApi.Workers
{
    public class CqrsWorker : CommonWorker
    {
        public CqrsWorker()
            : base(
                typeof(CqrsWorker).Assembly.GetResourceText(".Tau.WebApi.Cqrs.lisp", true),
                "cqrs-1.0", 
                true)
        {
        }
    }
}
