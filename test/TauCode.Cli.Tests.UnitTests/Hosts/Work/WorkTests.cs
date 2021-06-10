using NUnit.Framework;
using TauCode.Cli.Tests.Common.Hosts.Work;
using TauCode.Cli.Tests.Common.Hosts.Work.Mock;

namespace TauCode.Cli.Tests.UnitTests.Hosts.Work
{
    [TestFixture]
    public class WorkTests : HostTestBase
    {
        private MockBus _bus;

        public WorkTests()
        {
            _bus = new MockBus();
        }

        protected override ICliHost CreateHost() => new WorkerHost(_bus);

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
        public void Start_ValidWorkerName_StateIsRunning()
        {
            // Arrange
            var input = "start good-worker";

            // Act
            var command = this.Host.ParseCommand(input);
            this.Host.DispatchCommand(command);

            // Assert
            Assert.That(_bus.Worker.State, Is.EqualTo(MockWorkerState.Running));
        }

        [Test]
        public void Timeout_ValidWorkerNameNoTimeoutValue_ReturnsTimeout()
        {
            // Arrange
            var input = "timeout good-worker";

            // Act
            var command = this.Host.ParseCommand(input);
            this.Host.DispatchCommand(command);

            var output = this.GetOutput();

            // Assert
            Assert.That(output, Does.StartWith(_bus.Worker.Timeout.ToString()));
        }

        [Test]
        public void Timeout_ValidWorkerNameWithTimeoutValue_ReturnsTimeout()
        {
            // Arrange
            var input = "timeout good-worker 777";

            // Act
            var command = this.Host.ParseCommand(input);
            this.Host.DispatchCommand(command);

            var output = this.GetOutput();

            // Assert
            Assert.That(_bus.Worker.Timeout, Is.EqualTo(777));
            Assert.That(output, Does.StartWith("777"));
        }
    }
}
