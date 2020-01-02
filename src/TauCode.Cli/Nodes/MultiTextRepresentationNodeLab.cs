using System;
using System.Collections.Generic;
using TauCode.Parsing;
using TauCode.Parsing.Lab;
using TauCode.Parsing.Nodes;
using TauCode.Parsing.Tokens;

namespace TauCode.Cli.Nodes
{
    // todo with this 'Lab'
    public class MultiTextRepresentationNodeLab : ActionNode
    {
        private readonly HashSet<string> _textRepresentationVariants;
        private readonly HashSet<ITextClass> _textClasses;

        public MultiTextRepresentationNodeLab(
            IEnumerable<string> textRepresentationVariants,
            IEnumerable<ITextClass> textClasses,
            Action<IToken, IResultAccumulator> action,
            INodeFamily family,
            string name)
            : base(action, family, name)
        {
            if (textRepresentationVariants == null)
            {
                _textRepresentationVariants = null;
            }
            else
            {
                _textRepresentationVariants = new HashSet<string>(textRepresentationVariants);
            }

            if (textClasses == null)
            {
                // todo: also check collection for nulls.
                throw new ArgumentNullException(nameof(textClasses));
            }

            _textClasses = new HashSet<ITextClass>(textClasses);
        }

        protected override InquireResult InquireImpl(IToken token, IResultAccumulator resultAccumulator)
        {
            if (token is TextToken textToken && _textClasses.Contains(textToken.Class))
            {
                if (_textRepresentationVariants == null)
                {
                    // any representation does.
                    return this.Action == null ? InquireResult.Skip : InquireResult.Act;
                }
                else
                {
                    var representation = CliHelperLab.GetTextTokenRepresentation(textToken);
                    if (_textRepresentationVariants.Contains(representation))
                    {
                        return this.Action == null ? InquireResult.Skip : InquireResult.Act;
                    }
                }
            }

            return InquireResult.Reject;
        }
    }
}
