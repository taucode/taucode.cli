namespace TauCode.Cli.Tests.TestCli
{
    public class TestHost : CliHostBase
    {
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
        public override ICliAddIn[] CreateAddIns()
        {
            throw new System.NotImplementedException();
        }
    }
}
