using NUnit.Framework;
using TauCode.Cli.CliCommandEntries;
using TauCode.Extensions;

namespace TauCode.Cli.Tests
{
    [TestFixture]
    public class CliParserTests
    {
        [Test]
        public void Parse_ValidInput_ProducesExpectedResult()
        {
            // Arrange
            var grammar = this.GetType().Assembly.GetResourceText("cli-grammar.lisp", true);
            var parser = new CliParser(grammar);
            var commandText =
                "mm --conn=\"Server=.;Database=econera.diet.tracking;Trusted_Connection=True;\" --provider=sqlserver --to=sqlite --target-path=c:/temp/mysqlite.json -v";

            // Act
            var command = parser.Parse(commandText);

            // Assert
            Assert.That(command.Alias, Is.EqualTo("mm"));

            Assert.That(command.Entries, Has.Count.EqualTo(5));

            var keyValueEntry = (KeyValueCliCommandEntry)command.Entries[0];
            Assert.That(keyValueEntry.Alias, Is.EqualTo("connection").IgnoreCase);
            Assert.That(keyValueEntry.Key, Is.EqualTo("conn"));
            Assert.That(keyValueEntry.Value, Is.EqualTo("Server=.;Database=econera.diet.tracking;Trusted_Connection=True;"));

            keyValueEntry = (KeyValueCliCommandEntry)command.Entries[1];
            Assert.That(keyValueEntry.Alias, Is.EqualTo("provider").IgnoreCase);
            Assert.That(keyValueEntry.Key, Is.EqualTo("provider"));
            Assert.That(keyValueEntry.Value, Is.EqualTo("sqlserver"));

            keyValueEntry = (KeyValueCliCommandEntry)command.Entries[2];
            Assert.That(keyValueEntry.Alias, Is.EqualTo("target-provider").IgnoreCase);
            Assert.That(keyValueEntry.Key, Is.EqualTo("to"));
            Assert.That(keyValueEntry.Value, Is.EqualTo("sqlite"));

            keyValueEntry = (KeyValueCliCommandEntry)command.Entries[3];
            Assert.That(keyValueEntry.Alias, Is.EqualTo("target-path").IgnoreCase);
            Assert.That(keyValueEntry.Key, Is.EqualTo("target-path"));
            Assert.That(keyValueEntry.Value, Is.EqualTo("c:/temp/mysqlite.json"));

            var keyEntry = (KeyCliCommandEntry)command.Entries[4];
            Assert.That(keyEntry.Alias, Is.EqualTo("verbose").IgnoreCase);
            Assert.That(keyEntry.Key, Is.EqualTo("v"));
        }
    }
}
