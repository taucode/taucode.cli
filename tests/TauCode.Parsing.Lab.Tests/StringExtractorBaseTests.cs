using NUnit.Framework;
using System.Linq;
using TauCode.Parsing.Lab.Zeta.Tokens;
using TauCode.Parsing.Lab.Zeta.ZetaClasses;
using TauCode.Parsing.Lexing;
using TauCode.Parsing.Tokens.TextDecorations;

namespace TauCode.Parsing.Lab.Tests
{
    [TestFixture]
    public class StringExtractorBaseTests
    {
        [Test]
        public void TodoWat()
        {
            ILexer lexer = new StringLexer();
            var input = "\"some text\"";
            var tokens = lexer.Lexize(input);
            var token = tokens.Single();
            var zetaToken = (ZetaToken)token;
            Assert.That(zetaToken.Class, Is.TypeOf<StringZetaClass>());
            Assert.That(zetaToken.Decoration, Is.TypeOf<NoneTextDecoration>());
            Assert.That(zetaToken.Text, Is.EqualTo("some text"));
        }
    }
}
