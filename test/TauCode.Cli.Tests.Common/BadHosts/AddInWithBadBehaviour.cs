using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using TauCode.Cli.Commands;
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

        private class CustomExecutor : ICliExecutor
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

            public Task ProcessAsync(IList<CliCommandEntry> entries, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public CliExecutorDescriptor Descriptor { get; }
        }

        private class StandardExecutor : CliExecutorBase
        {
            public StandardExecutor()
                : base(
                    typeof(StandardExecutor).Assembly.GetResourceText(".BadHostResources.NamedExecutor.lisp", true),
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
            NullExecutors = 1,
            EmptyExecutors = 2,
            CustomExecutor = 3,
            GoodButNoName = 4,
        }

        private readonly BadBehaviour _behaviour;

        public AddInWithBadBehaviour(BadBehaviour behaviour)
        {
            _behaviour = behaviour;
        }

        protected override IReadOnlyList<ICliExecutor> CreateExecutors()
        {
            switch (_behaviour)
            {
                case BadBehaviour.CustomExecutor:
                    return new List<ICliExecutor>
                    {
                        new StandardExecutor(),
                        new CustomExecutor(),
                    };

                case BadBehaviour.EmptyExecutors:
                    return new List<ICliExecutor>();

                case BadBehaviour.NullExecutors:
                    return null;

                case BadBehaviour.GoodButNoName:
                    return new List<ICliExecutor>
                    {
                        new StandardExecutor(),
                    };

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
