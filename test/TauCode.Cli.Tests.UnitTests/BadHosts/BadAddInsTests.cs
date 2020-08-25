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
        public void Constructor_AddInWithCustomExecutors_ThrowsCliException()
        {
            // Arrange

            // Act
            var addIn = new AddInWithBadBehaviour(AddInWithBadBehaviour.BadBehaviour.CustomExecutor);
            INode dummy;
            var ex = Assert.Throws<CliException>(() => dummy = addIn.Node);

            // Assert
            Assert.That(ex.Message, Is.EqualTo("'CreateExecutors' must return instances of type 'TauCode.Cli.CliExecutorBase'."));
        }

        [Test]
        public void Constructor_AddInWithNullExecutors_ThrowsCliException()
        {
            // Arrange

            // Act
            var addIn = new AddInWithBadBehaviour(AddInWithBadBehaviour.BadBehaviour.NullExecutors);
            INode dummy;
            var ex = Assert.Throws<CliException>(() => dummy = addIn.Node);

            // Assert
            Assert.That(ex.Message, Is.EqualTo("'CreateExecutors' must not return null."));
        }

        [Test]
        public void Constructor_AddInWithEmptyExecutors_ThrowsCliException()
        {
            // Arrange

            // Act
            var addIn = new AddInWithBadBehaviour(AddInWithBadBehaviour.BadBehaviour.EmptyExecutors);
            INode dummy;
            var ex = Assert.Throws<CliException>(() => dummy = addIn.Node);

            // Assert
            Assert.That(ex.Message, Is.EqualTo("'CreateExecutors' must not return empty collection."));
        }

        [Test]
        public void GetHelp_AddInWithoutName_ThrowsCliException()
        {
            // Arrange

            // Act
            var addIn = new AddInWithBadBehaviour(AddInWithBadBehaviour.BadBehaviour.EmptyExecutors);
            string help;
            var ex = Assert.Throws<CliException>(() => help = addIn.GetHelp());
            
            // Assert
            Assert.That(ex.Message, Is.EqualTo("Help is not supported."));
        }
    }
}
