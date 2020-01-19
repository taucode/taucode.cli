using System.Collections.Generic;
using TauCode.Cli.Data;
using TauCode.Cli.Exceptions;
using TauCode.Parsing.Exceptions;

namespace TauCode.Cli
{
    /// <summary>
    /// Provides a piece of functionality, e.g. serializes SQL data within "db" add-in.
    /// </summary>
    public interface ICliWorker : ICliFunctionalityProvider
    {
        ICliAddIn AddIn { get; }
        FallbackInterceptedCliException HandleFallback(FallbackNodeAcceptedTokenException ex);
        void Process(IList<CliCommandEntry> entries);
        CliWorkerDescriptor Descriptor { get; }
    }
}
