using System.Collections.Generic;
using TauCode.Cli.Data;

namespace TauCode.Cli
{
    /// <summary>
    /// Provides a piece of functionality, e.g. serializes SQL data within "db" add-in.
    /// </summary>
    public interface ICliWorker : ICliFunctionalityProvider
    {
        ICliAddIn AddIn { get; }
        void Process(IList<CliCommandEntry> entries);
    }
}
