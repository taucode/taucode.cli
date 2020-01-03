using TauCode.Cli.TextClasses;
using TauCode.Extensions;
using TauCode.Parsing;
using TauCode.Parsing.Lexing;
using TauCode.Parsing.Tokens;
using TauCode.Parsing.Tokens.TextDecorations;

namespace TauCode.Cli.TokenExtractors
{
    public class PathExtractor : TokenExtractorBase
    {
        public PathExtractor(ILexingEnvironment environment)
            : base(environment, IsPathFirstChar)
        {
        }

        private static bool IsPathFirstChar(char c) =>
            LexingHelper.IsDigit(c) ||
            LexingHelper.IsLatinLetter(c) ||
            c.IsIn('\\', '/', '.', '!', '~', '$', '%', '-', '+', '_');

        private static bool IsPathSucceedingChar(char c) =>
            IsPathFirstChar(c) ||
            c.IsIn('=', ':', ';');

        protected override void ResetState()
        {
            // idle
        }

        protected override IToken ProduceResult()
        {
            var str = this.ExtractResultString();
            var token = new TextToken(PathTextClass.Instance, NoneTextDecoration.Instance, str);
            return token;
        }

        protected override CharChallengeResult ChallengeCurrentChar()
        {
            var c = this.GetCurrentChar();
            var pos = this.GetLocalPosition();

            if (pos == 0)
            {
                return CharChallengeResult.Continue; // 0th char MUST have been accepted.
            }

            if (IsPathSucceedingChar(c))
            {
                return CharChallengeResult.Continue;
            }

            if (this.Environment.IsSpace(c))
            {
                return CharChallengeResult.Finish;
            }

            return CharChallengeResult.GiveUp;
        }

        protected override CharChallengeResult ChallengeEnd()
        {
            return CharChallengeResult.Finish;
        }
    }
}
