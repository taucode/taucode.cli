using System;
using System.Collections.Generic;
using TauCode.Cli.Data;
using TauCode.Extensions;

namespace TauCode.Cli.Demo.Hosts.Kubectl.Scale
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

        public override void Process(IList<ICliCommandEntry> entries)
        {
            throw new NotImplementedException();
        }
    }
}
