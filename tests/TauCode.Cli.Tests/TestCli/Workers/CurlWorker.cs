using System.Collections.Generic;
using TauCode.Cli.Data;
using TauCode.Extensions;

namespace TauCode.Cli.Tests.TestCli.Workers
{
    public class CurlWorker : CliWorkerBase
    {
        public CurlWorker()
            : base(
                typeof(SerializeDataWorker).Assembly.GetResourceText("curl-grammar.lisp", true),
                "curl-1.0",
                true)
        {
        }

        public override void Process(IList<ICliCommandEntry> entries)
        {
            throw new System.NotImplementedException();
        }
    }
}
