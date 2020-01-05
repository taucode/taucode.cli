using System.Collections.Generic;
using TauCode.Cli.Data;

namespace TauCode.Cli
{
    public interface ICliHost : ICliFunctionalityProvider
    {
        IReadOnlyList<ICliAddIn> GetAddIns();
        CliCommand ParseCommand(params string[] input);
        void DispatchCommand(CliCommand command);
    }
}
