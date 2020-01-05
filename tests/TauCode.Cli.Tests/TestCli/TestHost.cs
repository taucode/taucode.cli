using System.Collections.Generic;

namespace TauCode.Cli.Tests.TestCli
{
    public class TestHost : CliHostBase
    {
        public TestHost()
            : base("test", "test-1488", true)
        {   
        }

        protected override IEnumerable<ICliAddIn> CreateAddIns()
        {
            return new ICliAddIn[]
            {
                    new TestDbAddIn(this),
            };
        }
    }
}
