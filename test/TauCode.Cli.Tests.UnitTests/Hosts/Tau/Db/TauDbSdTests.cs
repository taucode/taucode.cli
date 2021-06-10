using NUnit.Framework;
using System.Linq;
using TauCode.Cli.Exceptions;
using TauCode.Cli.Tests.Common.Hosts.Tau;
using TauCode.Cli.Tests.Common.Hosts.Tau.Db;
using TauCode.Cli.Tests.Common.Hosts.Tau.Db.Executors;
using TauCode.Parsing.Exceptions;

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
            SerializeDataExecutor.CurrentVersion = SerializeDataExecutor.DefaultVersion;
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
            var command = this.Host.ParseCommand(input);
            this.Host.DispatchCommand(command);
            var output = this.GetOutput();

            // Assert
            Assert.That(output, Is.EqualTo(@"Keys:
provider : sqlserver
exclude-table : table1, table2
Arguments:
connection-string : Server=.;Database=mydb;Trusted_Connection=True;
Options:

"));
        }

        [Test]
        [TestCase("db sd -p sqlserver 'my_conn' --verbose")]
        [TestCase("db sd -p sqlserver 'my_conn' -v")]
        public void SerializeData_OptionIsProvided_OptionIsProcessed(string input)
        {
            // Arrange

            // Act
            var command = this.Host.ParseCommand(input);
            this.Host.DispatchCommand(command);
            var output = this.GetOutput();

            // Assert
            Assert.That(output, Is.EqualTo(@"Keys:
provider : sqlserver
exclude-table : 
Arguments:
connection-string : my_conn
Options:
verbose

"));
        }

        [Test]
        public void SerializeData_BadKey_ThrowsFallbackInterceptedCliException()
        {
            // Arrange
            var input = "db sd -p sqlserver -bad-option badvalue 'my_conn' --verbose";

            // Act
            var ex = Assert.Throws<FallbackInterceptedCliException>(() => this.Host.ParseCommand(input));

            // Assert
            Assert.That(ex.Message, Is.EqualTo("Bad option or key: '-bad-option'."));
        }

        [Test]
        public void SerializeData_MissingKey_ThrowsCliException()
        {
            // Arrange
            var input = "db sd -e table1 --exclude \"table2\" 'Server=.;Database=mydb;Trusted_Connection=True;'";

            // Act
            var command = this.Host.ParseCommand(input);
            var ex = Assert.Throws<CliException>(() => this.Host.DispatchCommand(command));

            // Assert
            Assert.That(ex.Message, Is.EqualTo("Mandatory key with alias 'provider' (-p, --provider) was not provided."));
        }

        [Test]
        public void SerializeData_MultipleKey_ThrowsCliException()
        {
            // Arrange
            var input = "db sd -p sqlserver -p postgresql -e table1 --exclude \"table2\" 'Server=.;Database=mydb;Trusted_Connection=True;'";

            // Act
            var command = this.Host.ParseCommand(input);
            var ex = Assert.Throws<CliException>(() => this.Host.DispatchCommand(command));

            // Assert
            Assert.That(ex.Message, Is.EqualTo("Key with alias 'provider' (-p, --provider) does not allow multiple entries. Your provided: sqlserver, postgresql."));
        }

        [Test]
        public void SerializeData_MultipleOption_ThrowsCliException()
        {
            // Arrange
            var input = "db sd -p sqlserver 'my_conn' --verbose -v";

            // Act
            var command = this.Host.ParseCommand(input);
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
                this.Host.ParseCommand(input);
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
                this.Host.ParseCommand(input);
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
                this.Host.ParseCommand(input);
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
                    .GetExecutors()
                    .Single(x => x.Name.ToLowerInvariant() == "serialize-data")
                    .Version));
        }

        [Test]
        public void TauVersion_VersionDoesNotExist_ThrowsUnexpectedTokenException()
        {
            // Arrange
            this.Host = new TauHost(null, true);
            var input = "--version";
            
            // Act
            var ex = Assert.Throws<UnexpectedTokenException>(() => this.Host.ParseCommand(input));

            // Assert
            Assert.That(ex.Message, Is.EqualTo("Unexpected token: '--version'."));
        }

        [Test]
        public void TauDbVersion_VersionDoesNotExist_UnexpectedTokenException()
        {
            // Arrange
            DbAddIn.CurrentVersion = null;

            this.Host = new TauHost();
            var input = "db --version";

            // Act
            var ex = Assert.Throws<UnexpectedTokenException>(() => this.Host.ParseCommand(input));

            // Assert
            Assert.That(ex.Message, Is.EqualTo("Unexpected token: '--version'."));
        }

        [Test]
        public void TauDbSdVersion_VersionDoesNotExist_ThrowsFallbackInterceptedCliException()
        {
            // Arrange
            SerializeDataExecutor.CurrentVersion = null;

            this.Host = new TauHost
            {
                Output = this.Output
            };

            var input = "db sd --version";

            // Act
            var ex = Assert.Throws<FallbackInterceptedCliException>(() => this.Host.ParseCommand(input));

            // Assert
            Assert.That(ex.Message, Is.EqualTo("Bad option or key: '--version'."));
        }
    }
}
