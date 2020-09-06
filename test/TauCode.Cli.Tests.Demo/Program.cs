using TauCode.Cli.HostRunners;
using TauCode.Cli.Tests.Common.Hosts.Curl;
using TauCode.Cli.Tests.Common.Hosts.Git;
using TauCode.Cli.Tests.Common.Hosts.Kubectl;
using TauCode.Cli.Tests.Common.Hosts.Tau;
using TauCode.Cli.Tests.Common.Hosts.Work;
using TauCode.Cli.Tests.Common.Hosts.Work.Mock;

namespace TauCode.Cli.Tests.Demo
{
    class Program
    {
        static int Main(string[] args)
        {
            IBus mockBus = new MockBus();

            var runner = new DemoHostRunner(
                "idle",
                new ICliHost[]
                {
                    new TauHost(),
                    new GitHost(),
                    new CurlHost(),
                    new KubectlHost(),
                    new WorkerHost(mockBus),
                });

            var res = runner.Run(args);
            return res;
        }
    }
}
