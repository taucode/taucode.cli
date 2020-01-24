using System.Collections.Generic;
using TauCode.Cli.Data;

namespace TauCode.Cli
{
    public interface ICliHost : ICliFunctionalityProvider
    {
        IReadOnlyList<ICliAddIn> GetAddIns();
        CliCommand ParseCommand(string[] input);
        void DispatchCommand(CliCommand command);
    }
}
