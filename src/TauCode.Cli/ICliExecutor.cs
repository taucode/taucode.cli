using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TauCode.Cli.Commands;
using TauCode.Cli.Descriptors;
using TauCode.Cli.Exceptions;
using TauCode.Parsing.Exceptions;

namespace TauCode.Cli
{
    /// <summary>
    /// Provides a piece of functionality, e.g. serializes SQL data within "db" add-in.
    /// </summary>
    public interface ICliExecutor : ICliFunctionalityProvider
    {
        ICliAddIn AddIn { get; }
        FallbackInterceptedCliException HandleFallback(FallbackNodeAcceptedTokenException ex);
        CliExecutorDescriptor Descriptor { get; }
        void Process(IList<CliCommandEntry> entries);
        Task ProcessAsync(IList<CliCommandEntry> entries, CancellationToken cancellationToken = default);

    }
}
