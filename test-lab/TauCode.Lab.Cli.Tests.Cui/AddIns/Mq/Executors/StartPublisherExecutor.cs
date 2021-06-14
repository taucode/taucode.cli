using System;
using System.Collections.Generic;
using TauCode.Cli;
using TauCode.Cli.Commands;
using TauCode.Extensions;

namespace TauCode.Lab.Cli.Tests.Cui.AddIns.Mq.Executors
{
    public class StartPublisherExecutor : CliExecutorBase
    {
        public StartPublisherExecutor()
            : base(
                typeof(StartPublisherExecutor).Assembly.GetResourceText($"{nameof(StartPublisherExecutor)}.lisp", true),
                null,
                false)
        {
        }

        public override void Process(IList<CliCommandEntry> entries)
        {
            throw new NotImplementedException();
        }
    }
}
