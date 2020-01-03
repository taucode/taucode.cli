namespace TauCode.Cli.Tests
{
    public class TestProgram : CliProgramBase
    {
        public TestProgram()
            : base(
                "foo", 
                "foo descr", 
                true, 
                "1488",
                new ICliAddIn[]
                {
                    new TestDbAddIn(),
                })
        {
        }
    }
}
