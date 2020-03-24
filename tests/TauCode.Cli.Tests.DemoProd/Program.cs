using System;
using TauCode.Cli.HostRunners;
using TauCode.Cli.Tests.Common.Hosts.Tau;

namespace TauCode.Cli.Tests.DemoProd
{
    class Program
    {
        static int Main(string[] args)
        {
            var runner = new ProductionHostRunner(
                new TauHost
                {
                    Output = Console.Out,
                },
                true);

            var res = runner.Run(args);
            return res;
        }
    }
}
