using NUnit.Framework;
using System;

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
        public void SerializeData_BadKey_FallbackFires()
        {
            // Arrange

            // Act

            // Assert
            throw new NotImplementedException();
        }

        [Test]
        public void SerializeData_OrphanValue_ThrowsTodoException()
        {
            // Arrange

            // Act

            // Assert
            throw new NotImplementedException();
        }

        [Test]
        public void SerializeData_MissingKey_ThrowsTodoException()
        {
            // Arrange

            // Act

            // Assert
            throw new NotImplementedException();
        }

        [Test]
        public void SerializeData_MultipleKey_ThrowsTodoException()
        {
            // Arrange

            // Act

            // Assert
            throw new NotImplementedException();
        }

        [Test]
        public void SerializeData_MultipleOption_ThrowsTodoException()
        {
            // Arrange

            // Act

            // Assert
            throw new NotImplementedException();
        }

        [Test]
        public void TauVersion_VersionExists_VersionIsShown()
        {
            // Arrange

            // Act

            // Assert
            throw new NotImplementedException();
        }

        [Test]
        public void TauDbVersion_VersionExists_VersionIsShown()
        {
            // Arrange

            // Act

            // Assert
            throw new NotImplementedException();
        }

        [Test]
        public void TauDbSdVersion_VersionExists_VersionIsShown()
        {
            // Arrange

            // Act

            // Assert
            throw new NotImplementedException();
        }

        [Test]
        public void TauVersion_VersionDoesNotExist_ThrowsTodoException()
        {
            // Arrange

            // Act

            // Assert
            throw new NotImplementedException();
        }

        [Test]
        public void TauDbVersion_VersionDoesNotExist_ThrowsTodoException()
        {
            // Arrange

            // Act

            // Assert
            throw new NotImplementedException();
        }

        [Test]
        public void TauDbSdVersion_VersionDoesNotExist_ThrowsTodoException()
        {
            // Arrange

            // Act

            // Assert
            throw new NotImplementedException();
        }

    }
}
