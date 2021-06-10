using NUnit.Framework;
using TauCode.Cli.Exceptions;
using TauCode.Cli.Tests.Common.Hosts.Git;
using TauCode.Parsing.Exceptions;

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
            var command = this.Host.ParseCommand(input);
            this.Host.DispatchCommand(command);

            var output = this.GetOutput();

            // Assert
            Assert.That(output, Is.EqualTo(@"Keys: none
Arguments:
new-branch-name : feature/new-branch
base-branch-name : master
existing-branch-name : 
Options:
quiet
new-branch

"));

        }

        [Test]
        public void Checkout_ExistingBranch_ChecksOut()
        {
            // Arrange
            var input = "checkout feature/existing-branch";

            // Act
            var command = this.Host.ParseCommand(input);
            this.Host.DispatchCommand(command);

            var output = this.GetOutput();

            // Assert
            Assert.That(output, Is.EqualTo(@"Keys: none
Arguments:
new-branch-name : 
base-branch-name : 
existing-branch-name : feature/existing-branch
Options:

"));

        }

        [Test]
        public void Checkout_ExistingBranchRedundantArgument_ThrowsUnexpectedTokenException()
        {
            // Arrange
            var input = "checkout feature/existing-branch master";

            // Act
            var ex = Assert.Throws<UnexpectedTokenException>(() => this.Host.ParseCommand(input));

            // Assert
            Assert.That(ex.Token.ToString(), Is.EqualTo("master"));
        }

        [Test]
        public void Checkout_MissingBranch_ThrowsUnexpectedEndOfClauseException()
        {
            // Arrange
            var input = "checkout";

            // Act
            var ex = Assert.Throws<UnexpectedEndOfClauseException>(() => this.Host.ParseCommand(input));

            // Assert
        }

        [Test]
        public void Checkout_BadKey_ThrowsFallbackInterceptedCliException()
        {
            // Arrange
            var input = "checkout -bad-key";

            // Act
            var ex = Assert.Throws<FallbackInterceptedCliException>(() => this.Host.ParseCommand(input));
            
            // Assert
            Assert.That(ex.Message, Is.EqualTo("Bad key or option: -bad-key."));
        }

        [Test]
        public void Checkout_DuplicateOption_ThrowsCliException()
        {
            // Arrange
            var input = "checkout --quiet --quiet my-branch";

            // Act
            var command = this.Host.ParseCommand(input);
            var ex = Assert.Throws<CliException>(() => this.Host.DispatchCommand(command));

            // Assert
            Assert.That(ex.Message, Is.EqualTo("Multiple option. Alias: 'quiet'."));
        }
    }
}
