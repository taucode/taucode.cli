using NUnit.Framework;
using TauCode.Cli.Tests.TestCli;
using TauCode.Extensions.Lab;

namespace TauCode.Cli.Tests
{
    [TestFixture]
    public class CliProgramTests
    {
        [Test]
        public void Dispatch_ValidInput_ProducesExpectedResult()
        {
            var writer = new StringWriterWithEncodingLab();

            var host = new TestHost
            {
                Output = writer,
            };

            var input = new[]
            {
                "db",
                "sd",
                "--conn",
                "\"Server=.;Database=econera.diet.tracking;Trusted_Connection=True;\"",
                "--provider",
                "sqlserver",
                " --file",
                "c:/temp/mysqlite.json",
            };

            var command = host.ParseCommand(input);
            host.DispatchCommand(command);

            writer.Flush();

            var result = writer.GetStringBuilder().ToString();

            Assert.That(result, Is.EqualTo(
@"Serialize Data
Connection: Server=.;Database=econera.diet.tracking;Trusted_Connection=True;
Provider: sqlserver
File: c:/temp/mysqlite.json
"));
        }
    }
}
