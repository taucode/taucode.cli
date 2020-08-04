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
        public void Constructor_AddInWithCustomWorkers_ThrowsCliException()
        {
            // Arrange

            // Act
            var addIn = new AddInWithBadBehaviour(AddInWithBadBehaviour.BadBehaviour.CustomWorker);
            INode dummy;
            var ex = Assert.Throws<CliException>(() => dummy = addIn.Node);

            // Assert
            Assert.That(ex.Message, Is.EqualTo("'CreateWorkers' must return instances of type 'TauCode.Cli.CliWorkerBase'."));
        }

        [Test]
        public void Constructor_AddInWithNullWorkers_ThrowsCliException()
        {
            // Arrange

            // Act
            var addIn = new AddInWithBadBehaviour(AddInWithBadBehaviour.BadBehaviour.NullWorkers);
            INode dummy;
            var ex = Assert.Throws<CliException>(() => dummy = addIn.Node);

            // Assert
            Assert.That(ex.Message, Is.EqualTo("'CreateWorkers' must not return null."));
        }

        [Test]
        public void Constructor_AddInWithEmptyWorkers_ThrowsCliException()
        {
            // Arrange

            // Act
            var addIn = new AddInWithBadBehaviour(AddInWithBadBehaviour.BadBehaviour.EmptyWorkers);
            INode dummy;
            var ex = Assert.Throws<CliException>(() => dummy = addIn.Node);

            // Assert
            Assert.That(ex.Message, Is.EqualTo("'CreateWorkers' must not return empty collection."));
        }

        [Test]
        public void GetHelp_AddInWithoutName_ThrowsCliException()
        {
            // Arrange

            // Act
            var addIn = new AddInWithBadBehaviour(AddInWithBadBehaviour.BadBehaviour.EmptyWorkers);
            string help;
            var ex = Assert.Throws<CliException>(() => help = addIn.GetHelp());
            
            // Assert
            Assert.That(ex.Message, Is.EqualTo("Help is not supported."));
        }
    }
}
