using NUnit.Framework;
using TauCode.Cli.Exceptions;
using TauCode.Cli.Tests.Common.BadHosts;
using TauCode.Parsing;

namespace TauCode.Cli.Tests.UnitTests.BadHosts
{
    [TestFixture]
    public class BadAddInsTests
    {
        [Test]
        public void Constructor_HostWithoutAddIns_RunsOk()
        {
            // Arrange

            // Act
            var addIn = new AddInWithCustomWorkers();
            INode dummy;
            var ex = Assert.Throws<CliException>(() => dummy = addIn.Node);

            // Assert
            Assert.That(ex.Message, Is.EqualTo("'CreateWorkers' must return instances of type 'TauCode.Cli.CliWorkerBase'."));
        }
    }
}
