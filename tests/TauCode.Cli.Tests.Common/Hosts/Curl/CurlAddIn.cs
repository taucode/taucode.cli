﻿using System.Collections.Generic;

namespace TauCode.Cli.Tests.Common.Hosts.Curl
{
    /// <summary>
    /// Nameless add-in
    /// </summary>
    public class CurlAddIn : CliAddInBase
    {
        protected override IReadOnlyList<ICliWorker> CreateWorkers()
        {
            return new ICliWorker[]
            {
                new CurlWorker(),
            };
        }
    }
}