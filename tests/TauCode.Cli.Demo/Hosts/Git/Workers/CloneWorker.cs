using System;
using System.Collections.Generic;
using TauCode.Cli.Data;
using TauCode.Extensions;

namespace TauCode.Cli.Demo.Hosts.Git.Workers
{
    public class CloneWorker : CliWorkerBase
    {
        public CloneWorker()
            : base(
                typeof(CloneWorker).Assembly.GetResourceText(".Git.NoName.Clone.lisp", true),
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
