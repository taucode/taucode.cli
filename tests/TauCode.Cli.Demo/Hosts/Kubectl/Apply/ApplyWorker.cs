﻿using System;
using System.Collections.Generic;
using TauCode.Cli.Data;

namespace TauCode.Cli.Demo.Hosts.Kubectl.Apply
{
    public class ApplyWorker : CliWorkerBase
    {
        public ApplyWorker()
            : base("todo", null, true)
        {
        }

        public override void Process(IList<ICliCommandEntry> entries)
        {
            throw new NotImplementedException();
        }
    }
}
