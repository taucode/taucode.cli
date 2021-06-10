using System;
using System.Collections.Generic;

namespace TauCode.Lab.Data.Graphs
{
    public class Node<T> : INode<T>
    {
        #region Fields

        private readonly HashSet<Edge<T>> _outgoingEdges;
        private readonly HashSet<Edge<T>> _incomingEdges;

        #endregion

        #region Constructor

        public Node(T value)
        {
            this.Value = value;

            _outgoingEdges = new HashSet<Edge<T>>();
            _incomingEdges = new HashSet<Edge<T>>();
        }

        #endregion

        #region INode<T> Members

        public T Value { get; set; }

        public IEdge<T> DrawEdgeTo(INode<T> another)
        {
            if (another == null)
            {
                throw new ArgumentNullException(nameof(another));
            }

            var castedAnother = another as Node<T>;
            if (castedAnother == null)
            {
                throw new ArgumentException($"Expected node of type '{typeof(Node<T>).FullName}'.", nameof(another));
            }
            
            var edge = new Edge<T>(this, castedAnother);

            this._outgoingEdges.Add(edge);
            castedAnother._incomingEdges.Add(edge);

            return edge;
        }

        public IReadOnlyCollection<IEdge<T>> OutgoingEdges => _outgoingEdges;

        public IReadOnlyCollection<IEdge<T>> IncomingEdges => _incomingEdges;

        #endregion

        #region Internal

        internal void RemoveOutgoingEdge(Edge<T> outgoingEdge)
        {
            _outgoingEdges.Remove(outgoingEdge);
        }

        internal void RemoveIncomingEdge(Edge<T> incomingEdge)
        {
            _incomingEdges.Remove(incomingEdge);
        }

        #endregion
    }
}
