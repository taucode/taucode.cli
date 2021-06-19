using System;
using System.Diagnostics;

namespace TauCode.Lab.Data.Graphs.Rho.Impl
{
    [DebuggerDisplay("{From} -> {To}")]
    internal class RhoEdge : IRhoEdge
    {
        #region Fields

        private RhoNode _from;
        private RhoNode _to;
        private bool _isAlive;

        #endregion

        #region Constructor

        internal RhoEdge(RhoNode from, RhoNode to)
        {
            // arg checks omitted since the type is internal.

            _from = from;
            _to = to;
            _isAlive = true;
        }

        #endregion

        #region IRhoEdge Members

        public IRhoNode From
        {
            get
            {
                this.CheckAlive();

                return _from;
            }
        }

        public IRhoNode To
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
