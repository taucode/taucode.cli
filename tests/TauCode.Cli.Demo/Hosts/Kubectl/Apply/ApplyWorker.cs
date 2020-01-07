using System;
using System.Collections.Generic;
using TauCode.Cli.Data;

namespace TauCode.Cli.Demo.Hosts.Kubectl.Apply
{
    public class ApplyWorker : CliWorkerBase
    {
        public ApplyWorker(string grammar, string version, bool supportsHelp) : base(grammar, version, supportsHelp)
        {
        }

        public override void Process(IList<ICliCommandEntry> entries)
        {
            throw new NotImplementedException();
        }
    }
}
