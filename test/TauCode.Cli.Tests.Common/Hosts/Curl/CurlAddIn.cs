using System.Collections.Generic;

namespace TauCode.Cli.Tests.Common.Hosts.Curl
{
    /// <summary>
    /// Nameless add-in
    /// </summary>
    public class CurlAddIn : CliAddInBase
    {
        protected override IReadOnlyList<ICliExecutor> CreateWorkers()
        {
            return new ICliExecutor[]
            {
                new CurlWorker(),
            };
        }
    }
}
