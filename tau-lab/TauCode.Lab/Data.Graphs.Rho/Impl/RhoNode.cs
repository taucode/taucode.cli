using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace TauCode.Lab.Data.Graphs.Rho.Impl
{
    [DebuggerDisplay("{" + nameof(Name) + "}")]
    public class RhoNode : IRhoNode
    {
        #region Fields

        private readonly HashSet<RhoEdge> _outgoingEdges;
        private readonly HashSet<RhoEdge> _incomingEdges;

        #endregion

        #region Constructor

        public RhoNode()
        {
            _outgoingEdges = new HashSet<RhoEdge>();
            _incomingEdges = new HashSet<RhoEdge>();
        }

        #endregion

        #region IRhoNode Members

        public string Name { get; set; }

        public IRhoEdge DrawEdgeTo(IRhoNode another)
        {
            if (another == null)
            {
                throw new ArgumentNullException(nameof(another));
            }

            var castedAnother = another as RhoNode;
            if (castedAnother == null)
            {
                throw new ArgumentException($"Expected node of type '{typeof(RhoNode).FullName}'.", nameof(another));
            }

            var edge = new RhoEdge(this, castedAnother);

            this._outgoingEdges.Add(edge);
            castedAnother._incomingEdges.Add(edge);

            return edge;
        }

        public IReadOnlyCollection<IRhoEdge> OutgoingEdges => _outgoingEdges;

        public IReadOnlyCollection<IRhoEdge> IncomingEdges => _incomingEdges;

        #endregion

        #region Internal

        internal void RemoveOutgoingEdge(RhoEdge outgoingEdge)
        {
            _outgoingEdges.Remove(outgoingEdge);
        }

        internal void RemoveIncomingEdge(RhoEdge incomingEdge)
        {
            _incomingEdges.Remove(incomingEdge);
        }

        #endregion
    }
}
