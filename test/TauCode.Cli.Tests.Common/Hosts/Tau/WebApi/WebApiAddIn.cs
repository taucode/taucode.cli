using System.Collections.Generic;
using TauCode.Cli.Tests.Common.Hosts.Tau.WebApi.Workers;

namespace TauCode.Cli.Tests.Common.Hosts.Tau.WebApi
{
    public class WebApiAddIn : CliAddInBase
    {
        public WebApiAddIn()
            : base("web-api", "web-api-1.0", true)
        {

        }

        protected override IReadOnlyList<ICliExecutor> CreateWorkers()
        {
            return new ICliExecutor[]
            {
                new CqrsWorker(),
                new ControllerWorker(),
            };
        }
    }
}
