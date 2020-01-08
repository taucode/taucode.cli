using System.Collections.Generic;
using TauCode.Cli.Data;
using TauCode.Extensions;

namespace TauCode.Cli.Demo.Hosts.Curl
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

        public override void Process(IList<ICliCommandEntry> entries)
        {
            throw new System.NotImplementedException();
        }
    }
}
