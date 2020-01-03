using System.Collections.Generic;

namespace TauCode.Cli
{
    public interface ICliFunctionality
    {
        string Verb { get; }
        string Description { get; }
        bool SupportsHelp { get; }
        bool SupportsVersion { get; }
        string GetVersion();
        string GetHelp();
        IReadOnlyList<ICliSubCommandProcessor> Processors { get; }
    }
}
