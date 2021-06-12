using NUnit.Framework;
using TauCode.Lab.Data;

namespace TauCode.Lab.Tests.Data
{
    [TestFixture]
    public class VersionTests
    {
        [Test]
        [TestCase(0, 0, 1, null, "0.0.1")]
        [TestCase(0, 2, 7, null, "0.2.7")]
        [TestCase(3, 11, 16, null, "3.11.16")]
        [TestCase(7, 4, 8, "rc2", "7.4.8-rc2")]
        public void Ctor_ValidArguments_ProducesValidVersion(
            int major,
            int minor,
            int patch,
            string suffix,
            string expectedVersionString)
        {
            // Arrange
            
            // Act
            var version = new Version(major, minor, patch, suffix);

            // Assert
            Assert.That(version.Major, Is.EqualTo(major));
            Assert.That(version.Minor, Is.EqualTo(minor));
            Assert.That(version.Patch, Is.EqualTo(patch));
            Assert.That(version.Suffix, Is.EqualTo(suffix));

            Assert.That(version.ToString(), Is.EqualTo(expectedVersionString));
        }

        [Test]
        [TestCase("1-rc", "1.0-rc")]
        [TestCase("37.77", "37.77.0")]
        public void OperatorEquals_VersionsAreEqual_ReturnsTrue(string s1, string s2)
        {
            // Arrange
            var v1 = new Version(s1);
            var v2 = new Version(s2);

            // Act
            var eq = v1 == v2;

            // Assert
            Assert.That(eq, Is.True);
        }
    }
}
