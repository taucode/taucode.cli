using TauCode.Cli.HostRunners;
using TauCode.Cli.Tests.Common.Hosts.Curl;
using TauCode.Cli.Tests.Common.Hosts.Git;
using TauCode.Cli.Tests.Common.Hosts.Kubectl;
using TauCode.Cli.Tests.Common.Hosts.Tau;

namespace TauCode.Cli.Tests.Demo
{
    class Program
    {
        static int Main(string[] args)
        {
            var runner = new DemoHostRunner(
                "idle",
                new ICliHost[]
                {
                    new TauHost(),
                    new GitHost(),
                    new CurlHost(),
                    new KubectlHost(),
                });

            var res = runner.Run(args);
            return res;
        }
    }
}
