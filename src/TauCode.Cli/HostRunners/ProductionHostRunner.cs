using System;
using System.Linq;

namespace TauCode.Cli.HostRunners
{
    public class ProductionHostRunner : CliHostRunnerBase
    {
        private readonly ICliHost _host;
        private readonly bool _showLine;

        public ProductionHostRunner(ICliHost host, bool showLine = false)
        {
            _host = host ?? throw new ArgumentNullException(nameof(host));
            _showLine = showLine;
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

            if (_showLine)
            {
                _host.Output.WriteLine(line);
            }

            try
            {
                var command = _host.ParseLine(line);
                _host.DispatchCommand(command);
                return 0;
            }
            catch (Exception ex)
            {
                _host.Output.WriteLine(ex);
                return -1;
            }
        }
    }
}
