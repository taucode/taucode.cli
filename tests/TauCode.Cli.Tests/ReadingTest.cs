using System;
using NUnit.Framework;
using TauCode.Cli.Building;
using TauCode.Cli.Parsing;
using TauCode.Cli.Reading;

namespace TauCode.Cli.Tests
{
    [TestFixture]
    public class ReadingTest
    {
        [Test]
        public void Constructor_NotCompletedSyntax_ThrowsInvalidOperationException()
        {
            // Arrange
            var sb = new RootSyntaxBuilder()
                .AddDefaultCommand()
                .GetRoot();

            // Act
            var ex = Assert.Throws<InvalidOperationException>(() => new Reader(sb));

            // Assert
            Assert.That(ex.Message, Is.EqualTo("Cannot use not completed syntax."));
        }

        [Test]
        public void Read_DefaultCommandNoParameters_RunsOk()
        {
            // Arrange
            var sb = new RootSyntaxBuilder()
                .AddDefaultCommand()
                .GetRoot();

            sb.Complete();
            var input = "http://from.net http://to.net fast";
            var tokens = new Parser().ParseClause(input);

            // Act
            var command = new Reader(sb).Read(tokens);
            
            // Assert
            Assert.That(command.Name, Is.Null);
            Assert.That(command.NamedParameters, Is.Empty);

            Assert.That(command.ParameterValues, Has.Count.EqualTo(3));

            Assert.That(command.ParameterValues[0], Is.EqualTo("http://from.net"));
            Assert.That(command.ParameterValues[1], Is.EqualTo("http://to.net"));
            Assert.That(command.ParameterValues[2], Is.EqualTo("fast"));
        }

        [Test]
        public void Read_DefaultCommandWithParameters_RunsOk()
        {
            // Arrange
            var sb = new RootSyntaxBuilder()
                .AddDefaultCommand()
                    .AddNamedParameter("Verbose", "-v", "--verbose")
                    .Enum("true", "false")
                .GetRoot();

            sb.Complete();
            var input = "http://from.net http://to.net fast -v true close";
            var tokens = new Parser().ParseClause(input);

            // Act
            var command = new Reader(sb).Read(tokens);

            // Assert
            Assert.That(command.Name, Is.Null);
            Assert.That(command.NamedParameters, Has.Count.EqualTo(1));
            Assert.That(command.NamedParameters[0].Name, Is.EqualTo("Verbose"));
            Assert.That(command.NamedParameters[0].Value, Is.EqualTo("true"));

            Assert.That(command.ParameterValues, Has.Count.EqualTo(4));

            Assert.That(command.ParameterValues[0], Is.EqualTo("http://from.net"));
            Assert.That(command.ParameterValues[1], Is.EqualTo("http://to.net"));
            Assert.That(command.ParameterValues[2], Is.EqualTo("fast"));
            Assert.That(command.ParameterValues[3], Is.EqualTo("close"));
        }

        [Test]
        public void Read_UnknownCommand_ThrowsReadException()
        {
            // Arrange
            var sb = new RootSyntaxBuilder()
                .AddCommand("copy")
                    .AddNamedParameter("Verbose", "-v", "--verbose")
                    .Enum("true", "false")
                .GetRoot();

            sb.Complete();
            var input = "move http://from.net http://to.net fast -v true close";
            var tokens = new Parser().ParseClause(input);

            var reader = new Reader(sb);

            // Act
            var ex = Assert.Throws<ReadException>(() => reader.Read(tokens));

            // Assert
            Assert.That(ex.Message, Is.EqualTo("Unknown command: 'move'."));
        }

        [Test]
        public void Read_MandatoryNamedParameterNotProvided_ThrowsReadException()
        {
            // Arrange
            var sb = new RootSyntaxBuilder()
                .AddDefaultCommand()
                    .AddNamedParameter("Verbose", "-v", "--verbose").Mandatory()
                        .Enum("true", "false")
                    .AddNamedParameter("Color", "-c", "--color")
                        .Enum("white", "red")
                .GetRoot();

            sb.Complete();
            var input = "-c white";
            var tokens = new Parser().ParseClause(input);

            var reader = new Reader(sb);

            // Act
            var ex = Assert.Throws<ReadException>(() => reader.Read(tokens));

            // Assert
            Assert.That(ex.Message, Is.EqualTo("Value for mandatory named parameter '-v' was not provided."));
        }

        [Test]
        public void Read_ValueFormNamedParameterNotProvided_ThrowsReadException()
        {
            // Arrange
            var sb = new RootSyntaxBuilder()
                .AddDefaultCommand()
                    .AddNamedParameter("Verbose", "-v", "--verbose").Mandatory()
                        .Enum("true", "false")
                    .AddNamedParameter("Color", "-c", "--color")
                        .Enum("white", "red")
                .GetRoot();

            sb.Complete();
            var input = "-c white -v";
            var tokens = new Parser().ParseClause(input);

            var reader = new Reader(sb);

            // Act
            var ex = Assert.Throws<ReadException>(() => reader.Read(tokens));

            // Assert
            Assert.That(ex.Message, Is.EqualTo("Parameter '-v' requires a value."));
        }

        [Test]
        public void Read_ValueFormNamedParameterNotProvided2_ThrowsReadException()
        {
            // Arrange
            var sb = new RootSyntaxBuilder()
                .AddDefaultCommand()
                    .AddNamedParameter("Verbose", "-v", "--verbose").Mandatory()
                        .Enum("true", "false")
                    .AddNamedParameter("Color", "-c", "--color")
                        .Enum("white", "red")
                .GetRoot();

            sb.Complete();
            var input = "-v -c white";
            var tokens = new Parser().ParseClause(input);

            var reader = new Reader(sb);

            // Act
            var ex = Assert.Throws<ReadException>(() => reader.Read(tokens));

            // Assert
            Assert.That(ex.Message, Is.EqualTo("Parameter '-v' requires a value."));
        }

        [Test]
        public void Read_NoValueForValuelessParameter_RunsOk()
        {
            // Arrange
            var sb = new RootSyntaxBuilder()
                .AddDefaultCommand()
                    .AddNamedParameter("Color", "-c", "--color")
                        .Enum("white", "red")
                    .AddNamedParameter("Presence", "-p", "--presence")
                .GetRoot();

            sb.Complete();
            var input = "--color red -p";
            var tokens = new Parser().ParseClause(input);

            var reader = new Reader(sb);

            // Act
            var command = reader.Read(tokens);

            // Assert
            Assert.That(command.Name, Is.Null);

            Assert.That(command.ParameterValues, Is.Empty);

            Assert.That(command.NamedParameters, Has.Count.EqualTo(2));

            var param = command.NamedParameters[0];
            Assert.That(param.Name, Is.EqualTo("Color"));
            Assert.That(param.Value, Is.EqualTo("red"));

            param = command.NamedParameters[1];
            Assert.That(param.Name, Is.EqualTo("Presence"));
            Assert.That(param.Value, Is.Null);
        }

        [Test]
        public void Read_UnknownParameterName_ThrowsReadException()
        {
            // Arrange
            var sb = new RootSyntaxBuilder()
                .AddDefaultCommand()
                    .AddNamedParameter("Verbose", "-v", "--verbose").Mandatory()
                        .Enum("true", "false")
                    .AddNamedParameter("Color", "-c", "--color")
                        .Enum("white", "red")
                .GetRoot();

            sb.Complete();
            var input = "-v true -c white -wat";
            var tokens = new Parser().ParseClause(input);

            var reader = new Reader(sb);

            // Act
            var ex = Assert.Throws<ReadException>(() => reader.Read(tokens));

            // Assert
            Assert.That(ex.Message, Is.EqualTo("Unknown parameter: '-wat'."));
        }

        [Test]
        public void Read_UnknownParameterValue_ThrowsReadException()
        {
            // Arrange
            var sb = new RootSyntaxBuilder()
                .AddDefaultCommand()
                .AddNamedParameter("Verbose", "-v", "--verbose").Mandatory()
                .Enum("true", "false")
                .AddNamedParameter("Color", "-c", "--color")
                .Enum("white", "red")
                .GetRoot();

            sb.Complete();
            var input = "-v kuku -c white";
            var tokens = new Parser().ParseClause(input);

            var reader = new Reader(sb);

            // Act
            var ex = Assert.Throws<ReadException>(() => reader.Read(tokens));

            // Assert
            Assert.That(ex.Message, Is.EqualTo("Parameter '-v' won't accept value kuku"));
        }
    }
}
