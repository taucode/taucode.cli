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
            var url = entries.GetArgument("url");
            this.Output.WriteLine("Curl");
            this.Output.WriteLine($"Requested url: {url}");

            var headers = entries.GetAllValuesOfKey("header");
            this.Output.WriteLine($"Headers: {string.Join(", ", headers)}");
        }
    }
}
