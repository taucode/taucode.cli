using System;
using System.Collections.Generic;

namespace TauCode.Parsing.Lab.Zeta
{
    public struct EscapeSequenceExtractionResult
    {
        public EscapeSequenceExtractionResult(IList<char> chars, int positionShift, int lineShift, int? currentColumn)
        {
            if (chars == null)
            {
                var argsAreValid =
                    positionShift == 0 &&
                    lineShift == 0 &&
                    currentColumn == null;

                if (!argsAreValid)
                {
                    throw new ArgumentException("Inconsistent arguments."); // todo: arg name?
                }
            }
            else
            {
                var argsAreValid =
                    positionShift > 0 &&
                    lineShift >= 0 &&
                    currentColumn.HasValue &&
                    currentColumn.Value >= 0;

                if (!argsAreValid)
                {
                    throw new ArgumentException("Inconsistent arguments."); // todo: arg name?
                }
            }

            this.Chars = chars;
            this.PositionShift = positionShift;
            this.LineShift = lineShift;
            this.CurrentColumn = currentColumn;
        }

        public IList<char> Chars { get; }
        public int PositionShift { get; }
        public int LineShift { get; }
        public int? CurrentColumn { get; }
    }
}
