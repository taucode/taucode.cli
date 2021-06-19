using NUnit.Framework;
using TauCode.Lab.Data.Graphs.Rho;
using TauCode.Lab.Data.Graphs.Rho.Impl;

namespace TauCode.Lab.Tests.Algorithms.Graphs.Rho
{
    [TestFixture]
    public abstract class GraphTestBase
    {
        protected IRhoGraph Graph { get; set; }

        [SetUp]
        public void SetUpBase()
        {
            this.Graph = new RhoGraph();
        }

        [TearDown]
        public void TearDownBase()
        {
            this.Graph = null;
        }
    }
}
