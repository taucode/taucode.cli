using System;
using System.Collections.Generic;
using TauCode.Cli;
using TauCode.Cli.Data;
using TauCode.Extensions;

namespace TauCode.Lab.Cli.Tests.Cui.AddIns.LibDev.Executors
{
    public class DependenciesExecutor : CliExecutorBase
    {
        public DependenciesExecutor()
            : base(
                typeof(DependenciesExecutor).Assembly.GetResourceText($"{nameof(DependenciesExecutor)}.lisp", true),
                null,
                false)
        {
        }

        public override void Process(IList<CliCommandEntry> entries)
        {
            throw new NotImplementedException();
        }
    }
}
