using System.Collections.Generic;

namespace TauCode.Cli
{
    // todo clean up
    public interface ICliAddIn : ICliFunctionalityProvider
    {
        ICliHost Host { get; }
        //string Name { get; }
        //string Description { get; }
        //bool SupportsHelp { get; }
        //string GetVersion();
        //string GetHelp();
        IReadOnlyList<ICliWorker> GetWorkers();
    }
}
