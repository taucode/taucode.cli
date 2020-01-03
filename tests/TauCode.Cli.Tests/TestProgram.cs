using System.Collections.Generic;

namespace TauCode.Cli.Tests
{
    public class TestProgram : CliProgramBase
    {
        public TestProgram()
            : base(
                "foo",
                "foo descr",
                true,
                "1488")
        {
        }

        protected override IReadOnlyList<ICliAddIn> GetAddIns()
        {
            return new ICliAddIn[]
            {
                new TestDbAddIn(this), 
            };
        }
    }
}
