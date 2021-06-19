using System.Collections.Generic;

namespace TauCode.Lab.Data.Graphs.Rho
{
    public interface IRhoGraph
    {
        void AddNode(IRhoNode node);

        bool ContainsNode(IRhoNode node);

        bool RemoveNode(IRhoNode node);

        IReadOnlyCollection<IRhoNode> Nodes { get; }

        IEnumerable<IRhoEdge> Edges { get; }
    }
}
