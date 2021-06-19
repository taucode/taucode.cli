using System;
using System.Collections.Generic;
using System.Linq;

namespace TauCode.Lab.Data.Graphs.Rho.Impl
{
    public class RhoGraph : IRhoGraph
    {
        #region Fields

        private readonly HashSet<IRhoNode> _nodes;

        #endregion

        #region Constructor

        public RhoGraph()
        {
            _nodes = new HashSet<IRhoNode>();
        }

        #endregion

        #region IRhoGraph Members

        public void AddNode(IRhoNode node)
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

        public bool ContainsNode(IRhoNode node)
        {
            if (node == null)
            {
                throw new ArgumentNullException(nameof(node));
            }

            return _nodes.Contains(node);
        }

        public bool RemoveNode(IRhoNode node)
        {
            if (node == null)
            {
                throw new ArgumentNullException(nameof(node));
            }

            var removed = _nodes.Remove(node);

            return removed;
        }

        public IReadOnlyCollection<IRhoNode> Nodes => _nodes;

        public IEnumerable<IRhoEdge> Edges => this.Nodes.SelectMany(x => x.GetOutgoingEdgesLyingInGraph(this));

        #endregion
    }
}
