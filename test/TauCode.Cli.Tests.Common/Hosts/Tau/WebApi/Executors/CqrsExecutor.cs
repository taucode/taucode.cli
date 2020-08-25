using TauCode.Extensions;

namespace TauCode.Cli.Tests.Common.Hosts.Tau.WebApi.Executors
{
    public class CqrsExecutor : CommonExecutor
    {
        public CqrsExecutor()
            : base(
                typeof(CqrsExecutor).Assembly.GetResourceText(".Tau.WebApi.Cqrs.lisp", true),
                "cqrs-1.0", 
                true)
        {
        }
    }
}
