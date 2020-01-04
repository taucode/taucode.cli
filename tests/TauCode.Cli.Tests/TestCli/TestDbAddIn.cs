using System.Collections.Generic;

namespace TauCode.Cli.Tests.TestCli
{
    public class TestDbAddIn : CliAddInBase
    {
        public TestDbAddIn(ICliProgram program)
            : base(
                program,
                "db")
        {
        }

        public override IReadOnlyList<ICliProcessor> GetProcessors()
        {
            return new ICliProcessor[]
            {
                new TestSdProcessor(this), 
            };
        }
    }
}
