using System.Collections.Generic;
using TauCode.Cli.Data;

namespace TauCode.Cli.Demo.Hosts.Curl
{
    public class CurlWorker : CliWorkerBase
    {
        public CurlWorker()
            : base("todo", null, true)
        {
        }

        public override void Process(IList<ICliCommandEntry> entries)
        {
            throw new System.NotImplementedException();
        }
    }
}
