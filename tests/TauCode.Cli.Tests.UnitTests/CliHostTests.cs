using NUnit.Framework;
using TauCode.Cli.Tests.Common.Hosts.Tau;
using TauCode.Extensions;

namespace TauCode.Cli.Tests.UnitTests
{
    [TestFixture]
    public class CliHostTests
    {
        [Test]
        public void Dispatch_ValidInput_ProducesExpectedResult()
        {
            var writer = new StringWriterWithEncoding();

            var host = new TauHost
            {
                Output = writer,
            };

            var input = new[]
            {
                "db",
                "sd",
                "--connection",
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
