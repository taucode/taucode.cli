using NUnit.Framework;
using System;
using TauCode.Extensions;

namespace TauCode.Cli.Tests
{
    [TestFixture]
    public class CliParserTests
    {
        [Test]
        public void TodoWat()
        {
            // Arrange
            var grammar = this.GetType().Assembly.GetResourceText("cli-todo-wat.lisp", true);
            var parser = new CliParser(grammar);
            var commandText =
                "mm --conn=\"Server=.;Database=econera.diet.tracking;Trusted_Connection=True;\" --provider=sqlserver --to=sqlite --target-path=c:/temp/mysqlite.json -v";

            // Act
            var wat = parser.Parse(commandText);

            // Assert
            throw new NotImplementedException();
        }
    }
}
