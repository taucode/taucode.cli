using System.Collections.Generic;
using TauCode.Lab.Data.Graphs;
using TauCode.Lab.Dev.Data;

namespace TauCode.Lab.Cli.Tests.Cui.AddIns.LibDev.Data
{
    public class SolutionNode : Node<Solution>
    {
        public SolutionNode(Solution value)
            : base(value)
        {
        }

        public List<Project.PackageReference> Dependencies { get; set; }
    }
}
