using System.Collections.Generic;

namespace TauCode.Cli
{
    public interface ICliAddIn : ICliFunctionalityProvider
    {
        ICliHost Host { get; }
        IReadOnlyList<ICliExecutor> GetWorkers();
        string Description { get; }
    }
}
