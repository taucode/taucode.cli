using NUnit.Framework;
using System.Linq;
using TauCode.Parsing.Lexing;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Lab.Tests
{
    [TestFixture]
    public class MyLexerLabTests
    {
        [Test]
        public void TodoWat()
        {
            ILexer lexer = new MyLexerLab();
            var input = "  1488  ";
            var tokens = lexer.Lexize(input);
            var token = tokens.Single();
            var zetaToken = (IntegerToken)token;
            Assert.That(zetaToken.Value, Is.EqualTo("1488"));
            Assert.That(zetaToken.Position, Is.EqualTo(new Position(0, 2)));
            Assert.That(zetaToken.ConsumedLength, Is.EqualTo(4));
        }
    }
}
