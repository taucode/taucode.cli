using System.Collections.Generic;
using TauCode.Cli.Demo.AddIns.DbAddInProcessors;

namespace TauCode.Cli.Demo.AddIns
{
    public class DbAddIn : CliAddInBase
    {
        public DbAddIn(ICliProgram program)
            : base(program, "db")
        {
        }

        public override IReadOnlyList<ICliProcessor> GetProcessors()
        {
            return new ICliProcessor[]
            {
                new SdProcessor(this),
            };
        }
    }
}
