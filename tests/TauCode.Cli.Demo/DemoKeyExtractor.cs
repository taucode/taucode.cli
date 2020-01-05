//using TauCode.Cli.TextClasses;
//using TauCode.Parsing;
//using TauCode.Parsing.Lexing;
//using TauCode.Parsing.Tokens;
//using TauCode.Parsing.Tokens.TextDecorations;
// todo: clean up.
//namespace TauCode.Cli.Demo
//{
//    public class DemoKeyExtractor : TokenExtractorBase
//    {
//        public DemoKeyExtractor(ILexingEnvironment environment)
//            : base(environment, c => c == '-')
//        {
//        }

//        protected override void ResetState()
//        {
//        }

//        protected override IToken ProduceResult()
//        {
//            var str = this.ExtractResultString();
//            var token = new TextToken(KeyTextClass.Instance, NoneTextDecoration.Instance, str);
//            return token;
//        }

//        protected override CharChallengeResult ChallengeCurrentChar()
//        {
//            var c = this.GetCurrentChar();
//            var pos = this.GetLocalPosition();

//            if (pos == 0)
//            {
//                return CharChallengeResult.Continue; // 0th char MUST have been accepted.
//            }

//            if (pos == 1)
//            {
//                if (c == '-')
//                {
//                    //_skip = 2;
//                    //_textDecoration = HyphenTextDecoration.InstanceWithTwoHyphens;
//                    return CharChallengeResult.Continue;
//                }

//                if (LexingHelper.IsDigit(c) || LexingHelper.IsLatinLetter(c))
//                {
//                    return CharChallengeResult.Continue;
//                }

//                return CharChallengeResult.GiveUp;
//            }

//            if (pos == 2 && c == '-')
//            {
//                return CharChallengeResult.GiveUp; // 3 hyphens cannot be.
//            }

//            if (LexingHelper.IsDigit(c) || LexingHelper.IsLatinLetter(c) || c == '-')
//            {
//                return CharChallengeResult.Continue;
//            }

//            // todo: test keys "-", "--", "---", "--fo-", "-fo-", "---foo" etc.
//            if (this.Environment.IsSpace(c) || c == '=')
//            {
//                return CharChallengeResult.Finish;
//            }

//            return CharChallengeResult.GiveUp;
//        }

//        protected override CharChallengeResult ChallengeEnd()
//        {
//            var str = this.ExtractResultString();
//            if (str != "-" && str != "--")
//            {
//                return CharChallengeResult.Finish;
//            }

//            return CharChallengeResult.GiveUp;
//        }
//    }
//}
