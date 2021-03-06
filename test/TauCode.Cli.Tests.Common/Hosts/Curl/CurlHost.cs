﻿using System.Collections.Generic;
using System.Linq;

namespace TauCode.Cli.Tests.Common.Hosts.Curl
{
    public class CurlHost : CliHostBase
    {
        public CurlHost()
            : base("curl", "curl-1.0", true)
        {
        }

        protected override IReadOnlyList<ICliAddIn> CreateAddIns()
        {
            return new ICliAddIn[]
            {
                new CurlAddIn(),
            };
        }

        protected override string GetHelpImpl()
        {
            var executor = this
                .GetAddIns()
                .Single()
                .GetExecutors()
                .Single();
            var descriptor = executor.Descriptor;
            var help = descriptor.GetHelp();
            return help;
        }
    }
}
