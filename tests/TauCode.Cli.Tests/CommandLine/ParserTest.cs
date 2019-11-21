using System;
using NUnit.Framework;
using TauCode.Cli.Parsing;
using TauCode.Cli.Parsing.Tokens;

namespace TauCode.Cli.Tests.CommandLine
{
    [TestFixture]
    public class ParserTest
    {
        [Test]
        public void ParseClause_ValidClause_RunsOk()
        {
            // Arrange
            var input =
                " backup  -e sqlserver -c \"Server=.;Database=econera.messaging;Integrated Security=SSPI\" --path 'c:/temp/my-file.json' ";

            var parser = new Parser();

            // Act
            var tokens = parser.ParseClause(input);

            // Assert
            Assert.That(tokens, Has.Count.EqualTo(7));

            // 0
            var valueToken = (ValueToken)tokens[0];
            Assert.That(valueToken.Value, Is.EqualTo("backup"));

            // 1
            var parameterToken = (ParameterToken)tokens[1];
            Assert.That(parameterToken.ParameterAlias, Is.EqualTo("-e"));

            // 2
            valueToken = (ValueToken)tokens[2];
            Assert.That(valueToken.Value, Is.EqualTo("sqlserver"));

            // 3
            parameterToken = (ParameterToken)tokens[3];
            Assert.That(parameterToken.ParameterAlias, Is.EqualTo("-c"));

            // 4
            var quotedStringToken = (QuotedStringToken)tokens[4];
            Assert.That(quotedStringToken.OriginalValue, Is.EqualTo("\"Server=.;Database=econera.messaging;Integrated Security=SSPI\""));
            Assert.That(quotedStringToken.QuoteSymbol, Is.EqualTo('"'));
            Assert.That(quotedStringToken.UnquotedValue, Is.EqualTo("Server=.;Database=econera.messaging;Integrated Security=SSPI"));

            // 5
            parameterToken = (ParameterToken)tokens[5];
            Assert.That(parameterToken.ParameterAlias, Is.EqualTo("--path"));

            // 6
            quotedStringToken = (QuotedStringToken)tokens[6];
            Assert.That(quotedStringToken.OriginalValue, Is.EqualTo("'c:/temp/my-file.json'"));
            Assert.That(quotedStringToken.QuoteSymbol, Is.EqualTo('\''));
            Assert.That(quotedStringToken.UnquotedValue, Is.EqualTo("c:/temp/my-file.json"));
        }

        [Test]
        public void ParseClause_ArgumentIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            var parser = new Parser();

            
            // Act
            var ex = Assert.Throws<ArgumentNullException>(() => parser.ParseClause(null));

            // Assert
            Assert.That(ex.ParamName, Is.EqualTo("input"));
        }

        [Test]
        [TestCase("--verbose")]
        [TestCase(" --verbose")]
        [TestCase("--verbose ")]
        [TestCase(" --verbose ")]
        public void ParseClause_SingleParameterClause_RunsOk(string input)
        {
            // Arrange
            var parser = new Parser();

            // Act
            var tokens = parser.ParseClause(input);

            // Assert
            Assert.That(tokens, Has.Count.EqualTo(1));

            // 0
            var valueToken = (ParameterToken)tokens[0];
            Assert.That(valueToken.ParameterAlias, Is.EqualTo("--verbose"));
        }

        [Test]
        [TestCase("--verbose\"some\"")]
        [TestCase(" --verbose'some'")]
        public void ParseClause_UnexpectedQuoteWhenReadingParameter_ThrowsParsingException(string input)
        {
            // Arrange
            var parser = new Parser();

            // Act
            var ex = Assert.Throws<ParsingException>(() => parser.ParseClause(input));

            // Assert
            Assert.That(ex.Message, Is.EqualTo("Unexpected opening quote."));
        }

        [Test]
        [TestCase("--verbose?")]
        [TestCase(" --verboseдуже")]
        public void ParseClause_UnexpectedCharWhenReadingParameter_ThrowsParsingException(string input)
        {
            // Arrange
            var parser = new Parser();

            // Act
            var ex = Assert.Throws<ParsingException>(() => parser.ParseClause(input));

            // Assert
            Assert.That(ex.Message, Is.EqualTo("Unexpected character."));
        }

        [Test]
        [TestCase("sqlserver")]
        [TestCase(" sqlserver")]
        [TestCase("sqlserver ")]
        [TestCase(" sqlserver ")]
        public void ParseClause_SingleValueClause_RunsOk(string input)
        {
            // Arrange
            var parser = new Parser();

            // Act
            var tokens = parser.ParseClause(input);

            // Assert
            Assert.That(tokens, Has.Count.EqualTo(1));

            // 0
            var valueToken = (ValueToken)tokens[0];
            Assert.That(valueToken.Value, Is.EqualTo("sqlserver"));
        }

        [Test]
        [TestCase("sqlserver\"some\"")]
        [TestCase(" sqlserver'some'")]
        public void ParseClause_UnexpectedQuoteWhenReadingValue_ThrowsParsingException(string input)
        {
            // Arrange
            var parser = new Parser();

            // Act
            var ex = Assert.Throws<ParsingException>(() => parser.ParseClause(input));

            // Assert
            Assert.That(ex.Message, Is.EqualTo("Unexpected opening quote."));
        }

        [Test]
        [TestCase("\"non closed'")]
        [TestCase("\"non closed")]
        [TestCase("'non closed\"")]
        [TestCase("'non closed")]
        public void ParseClause_UnclosedQuote_ThrowsParsingException(string input)
        {
            // Arrange
            var parser = new Parser();

            // Act
            var ex = Assert.Throws<ParsingException>(() => parser.ParseClause(input));

            // Assert
            Assert.That(ex.Message, Is.EqualTo("Non-closed quote."));
        }

        [Test]
        [TestCase("\"one\"\"two\"")]
        [TestCase("'one''two'")]
        public void ParseClause_NoSpaceBeforeQuote_ThrowsParsingException(string input)
        {
            // Arrange
            var parser = new Parser();

            // Act
            var ex = Assert.Throws<ParsingException>(() => parser.ParseClause(input));

            // Assert
            Assert.That(ex.Message, Is.EqualTo("No space before opening quote."));
        }
    }
}
