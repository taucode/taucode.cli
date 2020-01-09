using System;
using TauCode.Parsing.Nodes;

namespace TauCode.Parsing.Lab.Zeta.Nodes
{
    public class MultiZetaNode : ActionNode
    {
        public MultiZetaNode(
            Action<ActionNode, IToken, IResultAccumulator> action,
            INodeFamily family,
            string name) : base(action, family, name)
        {
        }

        protected override InquireResult InquireImpl(IToken token, IResultAccumulator resultAccumulator)
        {
            throw new NotImplementedException();
        }
    }
}
