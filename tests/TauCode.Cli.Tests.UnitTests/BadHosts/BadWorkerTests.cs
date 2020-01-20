using NUnit.Framework;
using System;
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
            var ex = Assert.Throws<ArgumentException>(() => new WorkerWithNoNameButWithVersion());

            // Assert
            Assert.That(ex.ParamName, Is.EqualTo("version"));
            Assert.That(ex.Message, Does.StartWith("Nameless worker cannot support version."));
        }

        [Test]
        public void Constructor_NoNameButHasHelp_ThrowsCliException()
        {
            // Arrange

            // Act
            var ex = Assert.Throws<ArgumentException>(() => new WorkerWithNoNameButWithHelp());

            // Assert
            Assert.That(ex.ParamName, Is.EqualTo("supportsHelp"));
            Assert.That(ex.Message, Does.StartWith("Nameless worker cannot support help."));
        }
    }
}
