using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TauCode.Cli.Commands;

namespace TauCode.Cli
{
    public interface ICliHost : ICliFunctionalityProvider
    {
        IReadOnlyList<ICliAddIn> GetAddIns();
        CliCommand ParseCommand(string input);
        void DispatchCommand(CliCommand command);
        Task DispatchCommandAsync(CliCommand command, CancellationToken cancellationToken = default);
    }
}
