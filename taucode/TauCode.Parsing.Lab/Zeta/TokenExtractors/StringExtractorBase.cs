using System;
using System.Collections.Generic;
using System.Text;
using TauCode.Parsing.Lexing;

namespace TauCode.Parsing.Lab.Zeta.TokenExtractors
{
    public class StringExtractorBase : TokenExtractorBase
    {
        public StringExtractorBase(
            char openingChar,
            char closingChar,
            bool allowsLineBreaks,
            IList<IEscapeSequenceExtractor> escapeSequenceExtractors)
            : base(c => c == openingChar)
        {
            this.OpeningChar = openingChar;
            this.ClosingChar = closingChar;
            this.AllowsLineBreaks = allowsLineBreaks;
            // todo checks
            this.EscapeSequenceExtractors = escapeSequenceExtractors;
        }

        protected StringBuilder StringBuilder { get; private set; }
        protected char OpeningChar { get; private set; }
        protected char ClosingChar { get; private set; }
        protected IList<IEscapeSequenceExtractor> EscapeSequenceExtractors { get; private set; } // todo: optimize, check, hashtables/dictionaries, etc...
        protected bool AllowsLineBreaks { get; private set; }

        protected override void ResetState()
        {
            this.StringBuilder = new StringBuilder();
        }

        protected override IToken ProduceResult()
        {
            throw new NotImplementedException();
        }

        protected override CharChallengeResult ChallengeCurrentChar()
        {
            var c = this.GetCurrentChar();
            var index = this.LocalCharIndex;

            if (index == 0)
            {
                return CharChallengeResult.Continue;
            }
            
            // ask escape sequence extractors
            foreach (var extractor in this.EscapeSequenceExtractors)
            {
                throw new NotImplementedException();
            }

            throw new NotImplementedException();
        }

        protected override CharChallengeResult ChallengeEnd()
        {
            throw new NotImplementedException();
        }
    }
}
