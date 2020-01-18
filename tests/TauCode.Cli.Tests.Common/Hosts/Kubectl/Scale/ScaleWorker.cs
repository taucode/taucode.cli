using System.Collections.Generic;
using TauCode.Cli.Data;
using TauCode.Extensions;

namespace TauCode.Cli.Tests.Common.Hosts.Kubectl.Scale
{
    public class ScaleWorker : CliWorkerBase
    {
        public ScaleWorker()
            : base(
                typeof(ScaleWorker).Assembly.GetResourceText(".Kubectl.Scale.NoName.lisp", true),
                null,
                false)
        {
        }

        public override void Process(IList<CliCommandEntry> entries)
        {
            this.Output.WriteLine("Dummy implementation. Get back here when ready.");
        }
    }
}
