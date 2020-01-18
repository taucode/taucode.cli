using System.Collections.Generic;
using System.IO;
using TauCode.Cli.Data;
using TauCode.Cli.Exceptions;
using TauCode.Extensions;
using TauCode.Parsing;
using TauCode.Parsing.Exceptions;

namespace TauCode.Cli.Tests.Common.BadHosts
{
    public class AddInWithCustomWorkers : CliAddInBase
    {
        private class CustomWorker : ICliWorker
        {
            public string Name { get; }
            public TextWriter Output { get; set; }
            public TextReader Input { get; set; }
            public INode Node { get; }
            public string Version { get; }
            public bool SupportsHelp { get; }
            public string GetHelp()
            {
                return null;
            }

            public ICliAddIn AddIn { get; }
            public FallbackInterceptedCliException HandleFallback(FallbackNodeAcceptedTokenException ex)
            {
                return null;
            }

            public void Process(IList<CliCommandEntry> entries)
            {
                // void
            }
        }

        private class StandardWorker : CliWorkerBase
        {
            public StandardWorker()
                : base(
                    typeof(StandardWorker).Assembly.GetResourceText(".BadHostResources.NamedWorker.lisp", true),
                    null,
                    false)
            {
            }

            public override void Process(IList<CliCommandEntry> entries)
            {
                throw new System.NotImplementedException();
            }
        }

        protected override IReadOnlyList<ICliWorker> CreateWorkers()
        {
            return new List<ICliWorker>
            {
                new StandardWorker(),
                new CustomWorker(),
            };
        }
    }
}
