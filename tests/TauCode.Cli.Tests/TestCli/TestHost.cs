using System.Collections.Generic;

namespace TauCode.Cli.Tests.TestCli
{
    public class TestHost : CliHostBase
    {
        public TestHost()
            : base("test", "test-1488", true)
        {   
        }

        //public TestHost()
        //    : base(
        //        "foo",
        //        "foo descr",
        //        true,
        //        "1488")
        //{
        //}

        //protected override IReadOnlyList<ICliAddIn> GetAddIns()
        //{
        //    return new ICliAddIn[]
        //    {
        //        new TestDbAddIn(this), 
        //    };
        //}
        protected override IEnumerable<ICliAddIn> CreateAddIns()
        {
            throw new System.NotImplementedException();
        }
    }
}
