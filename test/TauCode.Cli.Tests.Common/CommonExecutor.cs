using System.Collections.Generic;
using TauCode.Cli.Commands;

namespace TauCode.Cli.Tests.Common
{
    public class CommonExecutor : CliExecutorBase
    {
        public CommonExecutor(string grammar, string version, bool supportsHelp)
            : base(grammar, version, supportsHelp)
        {
        }

        public override void Process(IList<CliCommandEntry> entries)
        {
            var summary = (new CliCommandSummaryBuilder()).Build(this.Descriptor, entries);
            this.Output.WriteLine(summary.FormatCommandSummary());
        }
    }
}
