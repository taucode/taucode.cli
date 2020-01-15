﻿using System;
using System.Collections.Generic;
using TauCode.Cli.Data;
using TauCode.Extensions;

namespace TauCode.Cli.Tests.TestCli.Workers
{
    public class SerializeDataWorker : CliWorkerBase
    {
        public SerializeDataWorker()
            : base(
                typeof(SerializeDataWorker).Assembly.GetResourceText("sd-grammar.lisp", true), 
                "sd-1.0", 
                true)
        {
        }

        public override void Process(IList<CliCommandEntry> entries)
        {
            throw new NotImplementedException();
            //var connection = ((KeyValueCliCommandEntry)entries.Single(x => x.Alias == "CONNECTION")).Value;
            //var provider = ((KeyValueCliCommandEntry)entries.Single(x => x.Alias == "PROVIDER")).Value;
            //var file = ((KeyValueCliCommandEntry)entries.Single(x => x.Alias == "FILE")).Value;

            //this.Output.WriteLine("Serialize Data");
            //this.Output.WriteLine($"Connection: {connection}");
            //this.Output.WriteLine($"Provider: {provider}");
            //this.Output.WriteLine($"File: {file}");
        }
    }
}
