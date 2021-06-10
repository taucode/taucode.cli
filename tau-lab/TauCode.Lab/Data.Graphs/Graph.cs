using System;
using System.Collections.Generic;

namespace TauCode.Lab.Data.Graphs
{
    public class Graph<T> : IGraph<T>
    {
        #region Fields

        private readonly HashSet<INode<T>> _nodes;

        #endregion

        #region Constructor

        public Graph()
        {
            _nodes = new HashSet<INode<T>>();
        }

        #endregion

        #region IGraph<T> Members

        public void AddNode(INode<T> node)
        {
            if (node == null)
            {
                throw new ArgumentNullException(nameof(node));
            }

            if (_nodes.Contains(node))
            {
                throw new ArgumentException("Graph already contains this node.", nameof(node));
            }

            _nodes.Add(node);
        }

        public bool ContainsNode(INode<T> node)
        {
            if (node == null)
            {
                throw new ArgumentNullException(nameof(node));
            }

            return _nodes.Contains(node);
        }

        public bool RemoveNode(INode<T> node)
        {
            if (node == null)
            {
                throw new ArgumentNullException(nameof(node));
            }

            var removed = _nodes.Remove(node);

            return removed;
        }

        public IReadOnlyCollection<INode<T>> Nodes => _nodes;

        #endregion
    }
}
