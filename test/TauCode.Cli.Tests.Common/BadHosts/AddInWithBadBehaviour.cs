using System;
using System.Collections.Generic;
using System.IO;
using TauCode.Cli.Data;
using TauCode.Cli.Descriptors;
using TauCode.Cli.Exceptions;
using TauCode.Extensions;
using TauCode.Parsing;
using TauCode.Parsing.Exceptions;

namespace TauCode.Cli.Tests.Common.BadHosts
{
    public class AddInWithBadBehaviour : CliAddInBase
    {
        #region Nested

        private class CustomWorker : ICliExecutor
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

            public CliWorkerDescriptor Descriptor { get; }
        }

        private class StandardWorker : CliExecutorBase
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
                // idle
            }
        }


        #endregion

        public enum BadBehaviour
        {
            NullWorkers = 1,
            EmptyWorkers = 2,
            CustomWorker = 3,
            GoodButNoName = 4,
        }

        private readonly BadBehaviour _behaviour;

        public AddInWithBadBehaviour(BadBehaviour behaviour)
        {
            _behaviour = behaviour;
        }

        protected override IReadOnlyList<ICliExecutor> CreateWorkers()
        {
            switch (_behaviour)
            {
                case BadBehaviour.CustomWorker:
                    return new List<ICliExecutor>
                    {
                        new StandardWorker(),
                        new CustomWorker(),
                    };

                case BadBehaviour.EmptyWorkers:
                    return new List<ICliExecutor>();

                case BadBehaviour.NullWorkers:
                    return null;

                case BadBehaviour.GoodButNoName:
                    return new List<ICliExecutor>
                    {
                        new StandardWorker(),
                    };

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
