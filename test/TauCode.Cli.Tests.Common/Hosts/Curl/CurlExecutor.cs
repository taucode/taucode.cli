using TauCode.Extensions;

namespace TauCode.Cli.Tests.Common.Hosts.Curl
{
    public class CurlExecutor : CommonExecutor
    {
        public CurlExecutor()
            : base(
                typeof(CurlExecutor).Assembly.GetResourceText(".Curl.NoName.NoName.lisp", true),
                null,
                false)
        {
        }
    }
}
