using NUnit.Framework;
using TauCode.Cli.Tests.Common.Hosts.Git;

namespace TauCode.Cli.Tests.UnitTests.Hosts.Git
{
    [TestFixture]
    public class GitTests : HostTestBase
    {
        protected override ICliHost CreateHost() => new GitHost();

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

        [Test]
        public void Checkout_NewBranch_ChecksOut()
        {
            // Arrange
            var input = "checkout --quiet -b feature/new-branch master";

            // Act
            var command = this.Host.ParseLine(input);
            this.Host.DispatchCommand(command);

            var output = this.GetOutput();

            // Assert
            Assert.That(output, Is.EqualTo(@"Git Checkout
Options: quiet, new-branch
Arguments:
new-branch-name = feature/new-branch
base-branch-name = master
"));

        }
    }
}
