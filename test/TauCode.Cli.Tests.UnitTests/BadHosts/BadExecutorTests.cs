using NUnit.Framework;
using System;
using TauCode.Cli.Tests.Common.BadHosts;

namespace TauCode.Cli.Tests.UnitTests.BadHosts
{
    [TestFixture]
    public class BadExecutorTests
    {
        [Test]
        public void Constructor_NoNameButHasVersion_ThrowsCliException()
        {
            // Arrange

            // Act
            var ex = Assert.Throws<ArgumentException>(() => new ExecutorWithNoNameButWithVersion());

            // Assert
            Assert.That(ex.ParamName, Is.EqualTo("version"));
            Assert.That(ex.Message, Does.StartWith("Nameless executor cannot support version."));
        }

        [Test]
        public void Constructor_NoNameButHasHelp_ThrowsCliException()
        {
            // Arrange

            // Act
            var ex = Assert.Throws<ArgumentException>(() => new ExecutorWithNoNameButWithHelp());

            // Assert
            Assert.That(ex.ParamName, Is.EqualTo("supportsHelp"));
            Assert.That(ex.Message, Does.StartWith("Nameless executor cannot support help."));
        }
    }
}
