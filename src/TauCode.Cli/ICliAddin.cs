using System.Collections.Generic;

namespace TauCode.Cli
{
    public interface ICliAddIn : ICliFunctionalityProvider
    {
        ICliHost Host { get; }
        IReadOnlyList<ICliExecutor> GetExecutors();
        string Description { get; }
        ICliContext Context { get; }
        void InitContext();
    }
}
