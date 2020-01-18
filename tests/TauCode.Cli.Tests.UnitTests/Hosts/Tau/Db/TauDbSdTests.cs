using NUnit.Framework;
using System.Linq;
using TauCode.Cli.Exceptions;
using TauCode.Cli.Tests.Common.Hosts.Tau;
using TauCode.Cli.Tests.Common.Hosts.Tau.Db;
using TauCode.Cli.Tests.Common.Hosts.Tau.Db.Workers;
using TauCode.Parsing.Lab.Exceptions;

namespace TauCode.Cli.Tests.UnitTests.Hosts.Tau.Db
{
    [TestFixture]
    public class TauDbSdTests : TauTestBase
    {
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            this.OneTimeSetUpBase();
        }

        [SetUp]
        public void SetUp()
        {
            DbAddIn.CurrentVersion = DbAddIn.DefaultVersion;
            SerializeDataWorker.CurrentVersion = SerializeDataWorker.DefaultVersion;
            this.SetUpBase();
        }

        [Test]
        [TestCase(
            "db sd -p sqlserver -e table1 --exclude \"table2\" 'Server=.;Database=mydb;Trusted_Connection=True;'")]
        [TestCase(
            "db sd --provider sqlserver --exclude 'table1' -e table2 \"Server=.;Database=mydb;Trusted_Connection=True;\"")]
        public void SerializeData_ValidInput_ProducesValidCommand(string input)
        {
            // Arrange

            // Act
            var command = this.Host.ParseLine(input);
            this.Host.DispatchCommand(command);
            var output = this.GetOutput();

            // Assert
            Assert.That(output, Is.EqualTo(@"Serialize Data
Provider: sqlserver; Excluded Tables: table1, table2; Connection String: Server=.;Database=mydb;Trusted_Connection=True;; 
"));
        }

        [Test]
        [TestCase("db sd -p sqlserver 'my_conn' --verbose")]
        [TestCase("db sd -p sqlserver 'my_conn' -v")]
        public void SerializeData_OptionIsProvided_OptionIsProcessed(string input)
        {
            // Arrange

            // Act
            var command = this.Host.ParseLine(input);
            this.Host.DispatchCommand(command);
            var output = this.GetOutput();

            // Assert
            Assert.That(output, Is.EqualTo(@"Serialize Data
Provider: sqlserver; Excluded Tables: ; Connection String: my_conn; Verbose; 
"));
        }

        [Test]
        public void SerializeData_BadKey_ThrowsFallbackInterceptedCliException()
        {
            // Arrange
            var input = "db sd -p sqlserver -bad-option badvalue 'my_conn' --verbose";

            // Act
            var ex = Assert.Throws<FallbackInterceptedCliException>(() => this.Host.ParseLine(input));

            // Assert
            Assert.That(ex.Message, Is.EqualTo("Bad option or key: '-bad-option'."));
        }

        [Test]
        public void SerializeData_MissingKey_ThrowsCliException()
        {
            // Arrange
            var input = "db sd -e table1 --exclude \"table2\" 'Server=.;Database=mydb;Trusted_Connection=True;'";

            // Act
            var command = this.Host.ParseLine(input);
            var ex = Assert.Throws<CliException>(() => this.Host.DispatchCommand(command));

            // Assert
            Assert.That(ex.Message, Is.EqualTo("Key is missing. Alias: 'provider'."));
        }

        [Test]
        public void SerializeData_MultipleKey_ThrowsCliException()
        {
            // Arrange
            var input = "db sd -p sqlserver -p postgresql -e table1 --exclude \"table2\" 'Server=.;Database=mydb;Trusted_Connection=True;'";

            // Act
            var command = this.Host.ParseLine(input);
            var ex = Assert.Throws<CliException>(() => this.Host.DispatchCommand(command));

            // Assert
            Assert.That(ex.Message, Is.EqualTo("Multiple keys. Alias: 'provider'."));
        }

        [Test]
        public void SerializeData_MultipleOption_ThrowsCliException()
        {
            // Arrange
            var input = "db sd -p sqlserver 'my_conn' --verbose -v";

            // Act
            var command = this.Host.ParseLine(input);
            var ex = Assert.Throws<CliException>(() => this.Host.DispatchCommand(command));

            // Assert
            Assert.That(ex.Message, Is.EqualTo("Multiple option. Alias: 'verbose'."));
        }

        [Test]
        public void TauVersion_VersionExists_VersionIsShown()
        {
            // Arrange
            var input = "--version";

            // Act
            try
            {
                this.Host.ParseLine(input);
            }
            catch (CliCustomHandlerException)
            {
                // dismiss
            }

            var output = this.GetOutput();

            // Assert
            Assert.That(output.Trim(), Is.EqualTo(this.Host.Version));
        }

        [Test]
        public void TauDbVersion_VersionExists_VersionIsShown()
        {
            // Arrange
            var input = "db --version";

            // Act
            try
            {
                this.Host.ParseLine(input);
            }
            catch (CliCustomHandlerException)
            {
                // dismiss
            }

            var output = this.GetOutput();

            // Assert
            Assert.That(output.Trim(), Is.EqualTo(this.Host.GetAddIns().Single(x => x.Name == "db").Version));
        }

        [Test]
        public void TauDbSdVersion_VersionExists_VersionIsShown()
        {
            // Arrange
            var input = "db sd --version";

            // Act
            try
            {
                this.Host.ParseLine(input);
            }
            catch (CliCustomHandlerException)
            {
                // dismiss
            }

            var output = this.GetOutput();

            // Assert
            Assert.That(
                output.Trim(), 
                Is.EqualTo(this.Host
                    .GetAddIns()
                    .Single(x => x.Name == "db")
                    .GetWorkers()
                    .Single(x => x.Name.ToLowerInvariant() == "serialize-data")
                    .Version));
        }

        [Test]
        public void TauVersion_VersionDoesNotExist_ThrowsUnexpectedTokenExceptionLab()
        {
            // Arrange
            this.Host = new TauHost(null, true);
            var input = "--version";
            
            // Act
            var ex = Assert.Throws<UnexpectedTokenExceptionLab>(() => this.Host.ParseLine(input));

            // Assert
            Assert.That(ex.Message, Is.EqualTo("Unexpected token: '--version'."));
        }

        [Test]
        public void TauDbVersion_VersionDoesNotExist_UnexpectedTokenExceptionLab() // todo: get rid of ..Lab everywhere.
        {
            // Arrange
            DbAddIn.CurrentVersion = null;

            this.Host = new TauHost();
            var input = "db --version";

            // Act
            var ex = Assert.Throws<UnexpectedTokenExceptionLab>(() => this.Host.ParseLine(input));

            // Assert
            Assert.That(ex.Message, Is.EqualTo("Unexpected token: '--version'."));
        }

        [Test]
        public void TauDbSdVersion_VersionDoesNotExist_ThrowsFallbackInterceptedCliException()
        {
            // Arrange
            SerializeDataWorker.CurrentVersion = null;

            this.Host = new TauHost
            {
                Output = this.Output
            };

            var input = "db sd --version";

            // Act
            var ex = Assert.Throws<FallbackInterceptedCliException>(() => this.Host.ParseLine(input));

            // Assert
            Assert.That(ex.Message, Is.EqualTo("Bad option or key: '--version'."));
        }
    }
}
