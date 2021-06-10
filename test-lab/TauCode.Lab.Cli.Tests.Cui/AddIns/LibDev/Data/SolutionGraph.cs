using System.Linq;
using TauCode.Lab.Data.Graphs;
using TauCode.Lab.Dev.Data;

namespace TauCode.Lab.Cli.Tests.Cui.AddIns.LibDev.Data
{
    public class SolutionGraph : Graph<Solution>
    {
        public void AddSolution(Solution solution)
        {
            this.AddNode(new SolutionNode(solution));
        }


        public SolutionNode FindSolutionNode(string solutionName)
        {
            return this.Nodes.SingleOrDefault(x => x.Value.Name == solutionName) as SolutionNode;
        }
    }
}
