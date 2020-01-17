using NUnit.Framework;

namespace TauCode.Cli.Tests.UnitTests.Hosts.Tau.Db
{
    [TestFixture]
    public class TauDbTests : TauTestBase
    {
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            this.OneTimeSetUpBase();
        }

        [SetUp]
        public void SetUp()
        {
            this.SetUpBase();
        }

        [Test]
        [TestCase("db sd -p sqlserver -e table1 --exclude \"table2\" 'Server=.;Database=mydb;Trusted_Connection=True;'")]
        [TestCase("db sd --provider sqlserver --exclude 'table1' -e table2 \"Server=.;Database=mydb;Trusted_Connection=True;\"")]
        public void SerializeData_ValidInput_ProducesValidCommand(string input)
        {
            // Arrange

            // Act
            var command = this.Host.ParseLine(input);
            this.Host.DispatchCommand(command);
            var output = this.GetOutput();

            // Assert
            Assert.That(output, Is.EqualTo(@"Serialize Data
Provider: sqlserver; Excluded Tables: table1, table2; Connection String: Server=.;Database=mydb;Trusted_Connection=True;
"));
        }
    }
}
