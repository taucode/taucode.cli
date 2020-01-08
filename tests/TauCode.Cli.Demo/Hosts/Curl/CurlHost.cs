using System.Collections.Generic;

namespace TauCode.Cli.Demo.Hosts.Curl
{
    public class CurlHost : CliHostBase
    {
        public CurlHost()
            : base("curl", "curl-1.0", true)
        {
        }

        protected override IReadOnlyList<ICliAddIn> CreateAddIns()
        {
            return new ICliAddIn[]
            {
                new CurlAddIn(),
            };
        }
    }
}
