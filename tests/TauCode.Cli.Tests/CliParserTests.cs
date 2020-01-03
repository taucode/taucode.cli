using NUnit.Framework;
using TauCode.Cli.Data.Entries;
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
                "sd --conn \"Server=.;Database=econera.diet.tracking;Trusted_Connection=True;\" --provider sqlserver --file c:/temp/mysqlite.json";

            // Act
            var command = parser.Parse(commandText);

            // Assert
            Assert.That(command.Alias, Is.EqualTo("serialize-data").IgnoreCase);

            Assert.That(command.Entries, Has.Count.EqualTo(3));

            var keyValueEntry = (KeyValueCliCommandEntry)command.Entries[0];
            Assert.That(keyValueEntry.Alias, Is.EqualTo("connection").IgnoreCase);
            Assert.That(keyValueEntry.Key, Is.EqualTo("conn"));
            Assert.That(keyValueEntry.Value, Is.EqualTo("Server=.;Database=econera.diet.tracking;Trusted_Connection=True;"));

            keyValueEntry = (KeyValueCliCommandEntry)command.Entries[1];
            Assert.That(keyValueEntry.Alias, Is.EqualTo("provider").IgnoreCase);
            Assert.That(keyValueEntry.Key, Is.EqualTo("provider"));
            Assert.That(keyValueEntry.Value, Is.EqualTo("sqlserver"));

            keyValueEntry = (KeyValueCliCommandEntry)command.Entries[2];
            Assert.That(keyValueEntry.Alias, Is.EqualTo("file").IgnoreCase);
            Assert.That(keyValueEntry.Key, Is.EqualTo("file"));
            Assert.That(keyValueEntry.Value, Is.EqualTo("c:/temp/mysqlite.json"));
        }
    }
}
