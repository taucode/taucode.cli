using NUnit.Framework;
using TauCode.Cli.Tests.Common.Hosts.Curl;

namespace TauCode.Cli.Tests.UnitTests.Hosts.Curl
{
    [TestFixture]
    public class CurlTests : HostTestBase
    {
        protected override ICliHost CreateHost() => new CurlHost();

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            this.OneTimeSetUpBase();
        }

        [SetUp]
        public void SetUp()
        {
            this.SetUpBase();
        }

    }
}
