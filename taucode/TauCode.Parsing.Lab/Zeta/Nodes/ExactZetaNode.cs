using System;
using System.Collections.Generic;
using TauCode.Parsing.Nodes;

namespace TauCode.Parsing.Lab.Zeta.Nodes
{
    public sealed class ExactZetaNode : ActionNode
    {
        public ExactZetaNode(
            string text,
            IList<IZetaClass> classes,
            Action<ActionNode, string, IResultAccumulator> textAction,
            INodeFamily family,
            string name)
            : base(null, family, name)
        {
            // todo: check args

            this.TextAction = textAction;
            // todo: Action in ActionNode should be virtual, to override it in current class, so you cannot set Action yourself in this class (ExactZetaNode)
            // todo: on-demand initialization, 'cause Action will be virtual and should not be called from ctor.
            // todo: same applies to other Zeta nodes.
            this.Action = ZetaAction;
        }

        public Action<ExactZetaNode, string, IResultAccumulator> TextAction { get; set; }

        private void ZetaAction(ActionNode node, IToken token, IResultAccumulator resultAccumulator)
        {
            throw new NotImplementedException();
        }

        protected override InquireResult InquireImpl(IToken token, IResultAccumulator resultAccumulator)
        {
            throw new NotImplementedException();
        }
    }
}
