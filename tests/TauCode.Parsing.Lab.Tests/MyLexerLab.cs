using System.Collections.Generic;
using TauCode.Parsing.Lab.Tests.TokenExtractors;
using TauCode.TextProcessing.Lab;

namespace TauCode.Parsing.Lab.Tests
{
    public class MyLexerLab : LexerBaseLab
    {
        protected override IList<IGammaTokenExtractor> CreateTokenExtractors()
        {
            return new IGammaTokenExtractor[]
            {
                new IntegerTokenExtractor(),
            };
        }

        protected override IList<ICharProcessor> CreateCharSkippers()
        {
            throw new System.NotImplementedException();
        }
    }
}
