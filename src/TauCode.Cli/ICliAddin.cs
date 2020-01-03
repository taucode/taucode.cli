using System.Collections.Generic;

namespace TauCode.Cli
{
    public interface ICliAddIn
    {
        string Name { get; }
        string Description { get; }
        bool SupportsHelp { get; }
        bool SupportsVersion { get; }
        //INode BuildNode();
        string GetVersion();
        string GetHelp();
        IReadOnlyList<ICliSubCommandProcessor> Processors { get; }
    }
}
