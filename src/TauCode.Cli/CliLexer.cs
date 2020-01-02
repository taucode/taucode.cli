using TauCode.Cli.TokenExtractors;
using TauCode.Parsing.Lexing;
using TauCode.Parsing.Lexing.StandardTokenExtractors;

namespace TauCode.Cli
{
    public class CliLexer : LexerBase
    {
        protected virtual TokenExtractorBase CreateIntegerExtractor() => new IntegerExtractor(this.Environment);
        protected virtual TokenExtractorBase CreateTermExtractor() => new TermExtractor(this.Environment);
        protected virtual TokenExtractorBase CreateKeyExtractor() => new KeyExtractor(this.Environment);
        protected virtual TokenExtractorBase CreateStringExtractor() => new StringExtractor(this.Environment);
        protected virtual TokenExtractorBase CreatePathExtractor() => new PathExtractor(this.Environment);
        protected virtual TokenExtractorBase CreateEqualsExtractor() => new EqualsExtractor(this.Environment);

        protected override void InitTokenExtractors()
        {
            // integer
            var integerExtractor = this.CreateIntegerExtractor();
            this.AddTokenExtractor(integerExtractor);

            // term
            var termExtractor = this.CreateTermExtractor();
            this.AddTokenExtractor(termExtractor);

            // key
            var keyExtractor = this.CreateKeyExtractor();
            this.AddTokenExtractor(keyExtractor);

            // string
            var stringExtractor = this.CreateStringExtractor();
            this.AddTokenExtractor(stringExtractor);

            // path
            var pathExtractor = this.CreatePathExtractor();
            this.AddTokenExtractor(pathExtractor);

            // equals
            var equalsExtractor = this.CreateEqualsExtractor();
            this.AddTokenExtractor(equalsExtractor);

            // *** Links ***
            keyExtractor.AddSuccessors(equalsExtractor);

            equalsExtractor.AddSuccessors(
                integerExtractor,
                termExtractor,
                keyExtractor,
                stringExtractor,
                pathExtractor);
        }
    }
}
