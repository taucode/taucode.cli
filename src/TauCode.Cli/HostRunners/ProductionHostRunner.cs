using System;
using System.Linq;

namespace TauCode.Cli.HostRunners
{
    public class ProductionHostRunner : CliHostRunnerBase
    {
        private readonly ICliHost _host;

        public ProductionHostRunner(ICliHost host)
        {
            _host = host ?? throw new ArgumentNullException(nameof(host));
        }

        public override int Run(string[] args)
        {
            if (args == null)
            {
                throw new ArgumentNullException(nameof(args));
            }

            if (args.Any(x => x == null))
            {
                throw new ArgumentException($"'{nameof(args)}' cannot contain nulls.");
            }

            var line = string.Join(" ", args);

            try
            {
                var command = _host.ParseLine(line);
                _host.DispatchCommand(command);
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return -1;
            }
        }
    }
}
