namespace TauCode.Cli.Tests
{
    public class TestDbAddIn : CliAddInImpl
    {
        public TestDbAddIn()
            : base(
                "db",
                new ICliSubCommandProcessor[]
                {
                    new TestSdProcessor(),
                })
        {
        }
    }
}
