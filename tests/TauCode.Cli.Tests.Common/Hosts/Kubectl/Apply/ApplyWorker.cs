using System.Collections.Generic;
using TauCode.Cli.Data;
using TauCode.Extensions;

namespace TauCode.Cli.Tests.Common.Hosts.Kubectl.Apply
{
    public class ApplyWorker : CliWorkerBase
    {
        public ApplyWorker()
            : base(
                typeof(ApplyWorker).Assembly.GetResourceText(".Kubectl.Apply.NoName.lisp", true),
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
