using System.Collections.Generic;
using TauCode.Cli.Demo.Hosts.Tau.WebApi.Workers;

namespace TauCode.Cli.Demo.Hosts.Tau.WebApi
{
    public class WebApiAddIn : CliAddInBase
    {
        public WebApiAddIn()
            : base("web-api", "web-api-1.0", true)
        {

        }

        protected override IReadOnlyList<ICliWorker> CreateWorkers()
        {
            return new ICliWorker[]
            {
                new CqrsWorker(),
                new ControllerWorker(),
            };
        }
    }
}
