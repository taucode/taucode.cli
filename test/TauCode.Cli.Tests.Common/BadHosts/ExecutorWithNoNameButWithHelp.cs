using System.Collections.Generic;
using TauCode.Cli.Commands;
using TauCode.Extensions;

namespace TauCode.Cli.Tests.Common.BadHosts
{
    public class ExecutorWithNoNameButWithHelp : CliExecutorBase
    {
        public ExecutorWithNoNameButWithHelp()
            : base(
                typeof(ExecutorWithNoNameButWithHelp).Assembly.GetResourceText(".BadHostResources.UnnamedExecutor.lisp", true),
                null,
                true)
        {
        }

        public override void Process(IList<CliCommandEntry> entries)
        {
            // idle
        }
    }
}
