using System;

namespace TauCode.Lab.Data.Graphs
{
    internal class Edge<T> : IEdge<T>
    {
        #region Fields

        private Node<T> _from;
        private Node<T> _to;
        private bool _isAlive;

        #endregion

        #region Constructor

        internal Edge(Node<T> from, Node<T> to)
        {
            // arg checks omitted since the type is internal.

            _from = from;
            _to = to;
            _isAlive = true;
        }

        #endregion

        #region IEdge<T> Members

        public INode<T> From
        {
            get
            {
                this.CheckAlive();

                return _from;
            }
        }

        public INode<T> To
        {
            get
            {
                this.CheckAlive();

                return _to;
            }
        }

        public void Disappear()
        {
            this.CheckAlive();

            _from.RemoveOutgoingEdge(this);
            _to.RemoveIncomingEdge(this);

            _from = null;
            _to = null;

            _isAlive = false;
        }

        #endregion

        #region Private

        private void CheckAlive()
        {
            if (!_isAlive)
            {
                throw new InvalidOperationException("Edge is not alive.");
            }
        }

        #endregion
    }
}
