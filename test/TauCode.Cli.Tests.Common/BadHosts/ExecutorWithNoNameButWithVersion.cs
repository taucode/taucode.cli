using System.Collections.Generic;
using TauCode.Cli.Commands;
using TauCode.Extensions;

namespace TauCode.Cli.Tests.Common.BadHosts
{
    public class ExecutorWithNoNameButWithVersion : CliExecutorBase
    {
        public ExecutorWithNoNameButWithVersion()
            : base(
                typeof(ExecutorWithNoNameButWithVersion).Assembly.GetResourceText(".BadHostResources.UnnamedExecutor.lisp", true),
                "has-version",
                false)
        {
        }

        public override void Process(IList<CliCommandEntry> entries)
        {
            // idle
        }
    }
}
