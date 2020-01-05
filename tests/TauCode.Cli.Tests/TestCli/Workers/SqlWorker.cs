using System;
using System.Collections.Generic;
using TauCode.Cli.Data;
using TauCode.Extensions;

namespace TauCode.Cli.Tests.TestCli.Workers
{
    public class SqlWorker : CliWorkerBase
    {
        public SqlWorker()
            : base(
                typeof(SerializeDataWorker).Assembly.GetResourceText("sd-grammar.lisp", true),
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
