using System;
using System.Collections.Generic;
using TauCode.Cli.Data;
using TauCode.Extensions;

namespace TauCode.Cli.Demo.Hosts.Kubectl.Apply
{
    public class ApplyWorker : CliWorkerBase
    {
        public ApplyWorker()
            : base(
                typeof(ApplyWorker).Assembly.GetResourceText(".Kubectl.Apply.NoName.lisp", true),
                null,
                true)
        {
        }

        public override void Process(IList<ICliCommandEntry> entries)
        {
            throw new NotImplementedException();
        }
    }
}
