using System;
using System.Linq;
using NUnit.Framework;
using TauCode.Cli.Building;

namespace TauCode.Cli.Tests
{
    [TestFixture]
    public class BuildingTest
    {
        #region AddCommand Tests

        [Test]
        public void AddCommand_NameIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            var rootSyntaxBuilder = new RootSyntaxBuilder();

            // Act
            var ex = Assert.Throws<ArgumentNullException>(() => rootSyntaxBuilder.AddCommand(null));

            // Assert
            Assert.That(ex.ParamName, Is.EqualTo("name"));
        }

        [Test]
        [TestCase(" cmd")]
        [TestCase("cmd ")]
        [TestCase("Cmd")]
        [TestCase("cmd!")]
        public void AddCommand_InvalidName_ThrowsArgumentException(string name)
        {
            // Arrange
            var rootSyntaxBuilder = new RootSyntaxBuilder();

            // Act
            var ex = Assert.Throws<ArgumentException>(() => rootSyntaxBuilder.AddCommand(name));

            // Assert
            Assert.That(ex.ParamName, Is.EqualTo("name"));
            Assert.That(ex.Message, Does.StartWith($"Bad command name: '{name}'."));
        }

        #endregion

        #region AddParameter Tests

        [Test]
        public void AddParameter_NameIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            var commandSyntaxBuilder = new RootSyntaxBuilder().AddCommand("run");

            // Act
            var ex = Assert.Throws<ArgumentNullException>(() =>
                commandSyntaxBuilder.AddNamedParameter(null, "-c", null, "--command"));

            // Assert
            Assert.That(ex.ParamName, Is.EqualTo("name"));
        }

        [Test]
        public void AddParameter_AliasesIsEmpty_ThrowsArgumentException()
        {
            // Arrange
            var commandSyntaxBuilder = new RootSyntaxBuilder().AddCommand("run");

            // Act
            var ex = Assert.Throws<ArgumentException>(() =>
                commandSyntaxBuilder.AddNamedParameter("Executable"));

            // Assert
            Assert.That(ex.ParamName, Is.EqualTo("aliases"));
            Assert.That(ex.Message, Does.StartWith("Aliases cannot be empty."));
        }

        [Test]
        [TestCase("param")]
        [TestCase(" -param")]
        [TestCase("-param ")]
        [TestCase("param!")]
        public void AddParameter_InvalidAlias_ThrowsArgumentException(string alias)
        {
            // Arrange
            var commandSyntaxBuilder = new RootSyntaxBuilder().AddCommand("run");

            // Act
            var ex = Assert.Throws<ArgumentException>(() =>
                commandSyntaxBuilder.AddNamedParameter("Name", "-c", alias, "--command"));

            // Assert
            Assert.That(ex.ParamName, Is.EqualTo("aliases"));
            Assert.That(ex.Message, Does.StartWith($"Invalid alias: '{alias}'"));
        }

        [Test]
        public void AddParameter_AliasIsNull_ThrowsArgumentException()
        {
            // Arrange
            var commandSyntaxBuilder = new RootSyntaxBuilder().AddCommand("run");

            // Act
            var ex = Assert.Throws<ArgumentException>(() =>
                commandSyntaxBuilder.AddNamedParameter("Name", "-c", null, "--command"));

            // Assert
            Assert.That(ex.ParamName, Is.EqualTo("aliases"));
            Assert.That(ex.Message, Does.StartWith("Alias cannot be null."));
        }

        [Test]
        public void AddParameter_DuplicateAlias_ThrowsArgumentException()
        {
            // Arrange
            var commandSyntaxBuilder = new RootSyntaxBuilder().AddCommand("run");

            // Act
            var ex = Assert.Throws<ArgumentException>(() =>
                commandSyntaxBuilder.AddNamedParameter("Name", "-c", "--command", "-c"));

            // Assert
            Assert.That(ex.ParamName, Is.EqualTo("aliases"));
            Assert.That(ex.Message, Does.StartWith("Duplicate alias: '-c'."));
        }

        #endregion

        #region Validate & Complete Tests

        [Test]
        public void Validate_DefaultCommandAddedTwice_ThrowsSyntaxException()
        {
            // Arrange
            var syntax1 = new RootSyntaxBuilder();
            syntax1.AddDefaultCommand();
            syntax1.AddDefaultCommand();

            var syntax2 = new RootSyntaxBuilder();
            syntax2.AddDefaultCommand();
            syntax2.AddCommand("run");

            var syntax3 = new RootSyntaxBuilder();
            syntax3.AddCommand("run");
            syntax3.AddDefaultCommand();

            // Act
            var ex1 = Assert.Throws<SyntaxException>(() => syntax1.Validate());
            var ex2 = Assert.Throws<SyntaxException>(() => syntax2.Validate());
            var ex3 = Assert.Throws<SyntaxException>(() => syntax3.Validate());

            // Assert
            Assert.That(ex1.Message, Is.EqualTo("If a default command is supplied, no other commands can be added."));
            Assert.That(ex2.Message, Is.EqualTo("If a default command is supplied, no other commands can be added."));
            Assert.That(ex3.Message, Is.EqualTo("If a default command is supplied, no other commands can be added."));
        }

        [Test]
        public void Validate_DuplicateCommand_ThrowsSyntaxException()
        {
            // Arrange
            var syntax = new RootSyntaxBuilder();
            syntax.AddCommand("run");
            syntax.AddCommand("run");

            // Act
            var ex = Assert.Throws<SyntaxException>(() => syntax.Validate());

            // Assert
            Assert.That(ex.Message, Is.EqualTo("Duplicate command: 'run'."));
        }

        [Test]
        public void Validate_DuplicateParameterName_ThrowsSyntaxException()
        {
            // Arrange
            var sb = new RootSyntaxBuilder()
                .AddCommand("backup")
                    .AddNamedParameter("Engine", "-e", "--engine").Mandatory()
                        .Enum("sqlserver", "postgresql")
                    .AddNamedParameter("Engine", "-c", "--connection").Mandatory()
                        .Any()
                .GetRoot();

            // Act
            var ex = Assert.Throws<SyntaxException>(() => sb.Validate());

            // Assert
            Assert.That(ex.Message, Is.EqualTo("Duplicate parameter name: 'Engine'."));
        }

        [Test]
        public void Validate_DuplicateParameterAliasDifferentParameters_ThrowsSyntaxException()
        {
            // Arrange
            var sb = new RootSyntaxBuilder()
                .AddCommand("backup")
                    .AddNamedParameter("Engine", "-e", "--engine").Mandatory()
                        .Enum("sqlserver", "postgresql")
                    .AddNamedParameter("Connection", "-c", "--connection", "-e").Mandatory()
                        .Any()
                .GetRoot();

            // Act
            var ex = Assert.Throws<SyntaxException>(() => sb.Validate());

            // Assert
            Assert.That(ex.Message, Is.EqualTo("Duplicate alias: '-e'."));
        }

        [Test]
        public void Complete_NotCompleted_Completes()
        {
            // Arrange
            var syntax = new RootSyntaxBuilder();
            syntax.AddCommand("run");

            // Act
            syntax.Complete();

            // Assert
            Assert.That(syntax.IsCompleted, Is.True);
        }

        [Test]
        public void Complete_AlreadyCompleted_ThrowsInvalidOperationException()
        {
            // Arrange
            var syntax = new RootSyntaxBuilder();
            syntax.AddCommand("run");
            syntax.Complete();

            // Act
            var ex = Assert.Throws<InvalidOperationException>(() => syntax.Complete());

            // Assert
            Assert.That(ex.Message, Is.EqualTo("Completed syntax cannot be modified."));
        }

        [Test]
        public void Modify_Completed_ThrowsIn()

        {
            // Arrange
            var syntax = new RootSyntaxBuilder();
            var command = syntax.AddCommand("run");
            var param = command.AddNamedParameter("par", "-p1");
            syntax.Complete();

            // Act
            var ex1 = Assert.Throws<InvalidOperationException>(() => syntax.AddCommand("some"));
            var ex2 = Assert.Throws<InvalidOperationException>(() => syntax.AddDefaultCommand());
            var ex3 = Assert.Throws<InvalidOperationException>(() => command.AddNamedParameter("other", "-o"));
            var ex4 = Assert.Throws<InvalidOperationException>(() => param.Mandatory());
            var ex5 = Assert.Throws<InvalidOperationException>(() => param.Enum("a1", "a2"));
            var ex6 = Assert.Throws<InvalidOperationException>(() => param.Any());

            var exes = new[] { ex1, ex2, ex3, ex4, ex5, ex6 };

            // Assert
            Assert.That(exes.All(x => x.Message == "Completed syntax cannot be modified."), Is.True);
        }

        #endregion
    }
}
