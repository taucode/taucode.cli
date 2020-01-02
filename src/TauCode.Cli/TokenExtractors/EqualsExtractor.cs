using System;
using TauCode.Parsing;
using TauCode.Parsing.Lexing;
using TauCode.Parsing.Tokens;

namespace TauCode.Cli.TokenExtractors
{
    public class EqualsExtractor : TokenExtractorBase
    {
        public EqualsExtractor(ILexingEnvironment environment)
            : base(environment, c => c == '=')
        {
        }

        protected override void ResetState()
        {
            // idle
        }

        protected override IToken ProduceResult()
        {
            return new PunctuationToken('=');
        }

        protected override CharChallengeResult ChallengeCurrentChar()
        {
            var pos = this.GetLocalPosition();

            if (pos == 0)
            {
                // 0th is always accepted.
                return CharChallengeResult.Continue;
            }

            return CharChallengeResult.Finish;
        }

        protected override CharChallengeResult ChallengeEnd()
        {
            throw new NotImplementedException();
        }
    }
}
