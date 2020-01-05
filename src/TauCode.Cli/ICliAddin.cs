using System.Collections.Generic;

namespace TauCode.Cli
{
    public interface ICliAddIn
    {
        ICliProgram Program { get; }
        string Name { get; }
        string Description { get; }
        bool SupportsHelp { get; }
        string GetVersion();
        string GetHelp();
        IReadOnlyList<ICliWorker> GetWorkers();
    }
}
