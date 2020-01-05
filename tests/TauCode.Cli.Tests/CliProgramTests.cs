using NUnit.Framework;
using TauCode.Cli.Tests.TestCli;
using TauCode.Extensions.Lab;

namespace TauCode.Cli.Tests
{
    [TestFixture]
    public class CliProgramTests
    {
        [Test]
        public void WatTodo()
        {
            var writer = new StringWriterWithEncodingLab();

            var program = new TestHost
            {
                Output = writer,
                Arguments = new []
                {
                    "db",
                    "sd",
                    "--conn",
                    "\"Server=.;Database=econera.diet.tracking;Trusted_Connection=True;\"",
                    "--provider",
                    "sqlserver",
                    " --file",
                    "c:/temp/mysqlite.json",
                },
            };
            var exitCode = program.Run();
            writer.Flush();

            var result = writer.GetStringBuilder().ToString();

            Assert.That(exitCode, Is.EqualTo(0));
            Assert.That(result, Is.EqualTo(
@"Serialize Data
Connection: Server=.;Database=econera.diet.tracking;Trusted_Connection=True;
Provider: sqlserver
File: c:/temp/mysqlite.json
"));
        }
    }
}
