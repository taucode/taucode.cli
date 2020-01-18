using NUnit.Framework;
using TauCode.Cli.Exceptions;
using TauCode.Cli.Tests.Common.BadHosts;

namespace TauCode.Cli.Tests.UnitTests.BadHosts
{
    [TestFixture]
    public class BadWorkerTests
    {
        [Test]
        public void Constructor_NoNameButHasVersion_ThrowsCliException()
        {
            // Arrange

            // Act
            var ex = Assert.Throws<CliException>(() => new WorkerWithNoNameButWithVersion());

            // Assert
            Assert.That(ex.Message, Is.EqualTo("Nameless worker cannot support version."));
        }

        [Test]
        public void Constructor_NoNameButHasHelp_ThrowsCliException()
        {
            // Arrange

            // Act
            var ex = Assert.Throws<CliException>(() => new WorkerWithNoNameButWithHelp());

            // Assert
            Assert.That(ex.Message, Is.EqualTo("Nameless worker cannot support help."));
        }

    }
}
