using System;
using System.Collections.Generic;
using TauCode.Parsing.Nodes;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Lab
{
    public class MultiTextRepresentationNodeLab : ActionNode
    {
        public MultiTextRepresentationNodeLab(
            IEnumerable<string> textRepresentationVariants,
            IEnumerable<ITextClass> textClasses,
            Action<IToken, IResultAccumulator> action,
            INodeFamily family,
            string name)
            : base(action, family, name)
        {

        }

        protected override InquireResult InquireImpl(IToken token, IResultAccumulator resultAccumulator)
        {
            throw new NotImplementedException();
        }
    }
}
