using System.Collections.Generic;
using TauCode.Cli.Data;
using TauCode.Extensions;

namespace TauCode.Cli.Tests.Common.Hosts.Curl
{
    public class CurlWorker : CliWorkerBase
    {
        public CurlWorker()
            : base(
                typeof(CurlWorker).Assembly.GetResourceText(".Curl.NoName.NoName.lisp", true),
                null,
                false)
        {
        }

        public override void Process(IList<CliCommandEntry> entries)
        {
            throw new System.NotImplementedException();
        }
    }
}
