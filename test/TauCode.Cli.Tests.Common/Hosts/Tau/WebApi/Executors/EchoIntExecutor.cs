using TauCode.Extensions;

namespace TauCode.Cli.Tests.Common.Hosts.Tau.WebApi.Executors
{
    public class EchoIntExecutor : CommonExecutor
    {
        public EchoIntExecutor()
            : base(
                typeof(CqrsExecutor).Assembly.GetResourceText(".Tau.WebApi.EchoInt.lisp", true),
                null, 
                true)
        {
        }
    }
}
