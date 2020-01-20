using System.Collections.Generic;

namespace TauCode.Cli
{
    public interface ICliAddIn : ICliFunctionalityProvider
    {
        ICliHost Host { get; }
        IReadOnlyList<ICliWorker> GetWorkers();
        string Description { get; }
    }
}
