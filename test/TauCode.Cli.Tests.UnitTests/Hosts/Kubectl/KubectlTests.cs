using NUnit.Framework;
using TauCode.Cli.Tests.Common.Hosts.Kubectl;

namespace TauCode.Cli.Tests.UnitTests.Hosts.Kubectl
{
    [TestFixture]
    public class KubectlTests : HostTestBase
    {
        protected override ICliHost CreateHost() => new KubectlHost();

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
