using System.Collections.Generic;

namespace TauCode.Lab.Data.Graphs.Rho
{
    public interface IRhoNode
    {
        string Name { get; set; }

        IRhoEdge DrawEdgeTo(IRhoNode another);

        IReadOnlyCollection<IRhoEdge> OutgoingEdges { get; }

        IReadOnlyCollection<IRhoEdge> IncomingEdges { get; }
    }
}