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
            var input = "1488";
            var tokens = lexer.Lexize(input);
            var token = tokens.Single();
            var zetaToken = (IntegerToken)token;
            Assert.That(zetaToken.Value, Is.EqualTo("1488"));
            //Assert.That(zetaToken.Class, Is.TypeOf<StringZetaClass>());
            //Assert.That(zetaToken.Decoration, Is.TypeOf<NoneTextDecoration>());
            //Assert.That(zetaToken.Text, Is.EqualTo("some text"));
        }
    }
}
