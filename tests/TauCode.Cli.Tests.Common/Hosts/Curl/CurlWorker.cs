using TauCode.Extensions;

namespace TauCode.Cli.Tests.Common.Hosts.Curl
{
    public class CurlWorker : CommonWorker
    {
        public CurlWorker()
            : base(
                typeof(CurlWorker).Assembly.GetResourceText(".Curl.NoName.NoName.lisp", true),
                null,
                false)
        {
        }
    }
}
