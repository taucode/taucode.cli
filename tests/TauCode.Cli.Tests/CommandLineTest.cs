using NUnit.Framework;
using TauCode.Cli.Building;
using TauCode.Cli.Parsing;
using TauCode.Cli.Reading;

namespace TauCode.Cli.Tests
{
    [TestFixture]
    public class CommandLineTest
    {
        [Test]
        public void Execute_HappyPath_RunsOk()
        {
            // Arrange
            var sb = new RootSyntaxBuilder()
                .AddCommand("backup")
                    .AddNamedParameter("Engine", "-e", "--engine").Mandatory()
                        .Enum("sqlserver", "postgresql")
                    .AddNamedParameter("Connection", "-c",  "--connection").Mandatory()
                        .Any()
                    .AddNamedParameter("Path", "-p", "--path").Mandatory()
                        .Any()
                    .AddNamedParameter("Format", "-f", "--format").Mandatory()
                        .Enum("json")
                        .Default("json")
                .AddCommand("restore")
                    .AddNamedParameter("Engine", "-e", "--engine").Mandatory()
                        .Enum("sqlserver", "postgresql")
                    .AddNamedParameter("Connection", "-c", "--connection").Mandatory()
                        .Any()
                    .AddNamedParameter("Path", "-p", "--path").Mandatory()
                        .Any()
                    .AddNamedParameter("Format", "-f", "--format").Mandatory()
                        .Enum("json")
                        .Default("json")
                .GetRoot();

            sb.Complete();

            var line =
                "backup -e sqlserver -c 'Server=.;Database=econera.messaging;Integrated Security=SSPI' -p 'c:/temp/my-file.json'";
            var tokens = new Parser().ParseClause(line);
            var reader = new Reader(sb);

            // Act
            var command = reader.Read(tokens);
            

            // Assert
            Assert.That(command.Name, Is.EqualTo("backup"));

            Assert.That(command.GetSingleParameter("Engine").Value, Is.EqualTo("sqlserver"));
            Assert.That(command.GetSingleParameter("Connection").Value, Is.EqualTo("Server=.;Database=econera.messaging;Integrated Security=SSPI"));
            Assert.That(command.GetSingleParameter("Path").Value, Is.EqualTo("c:/temp/my-file.json"));
            Assert.That(command.GetSingleParameter("Format").Value, Is.EqualTo("json"));
        }
    }
}
