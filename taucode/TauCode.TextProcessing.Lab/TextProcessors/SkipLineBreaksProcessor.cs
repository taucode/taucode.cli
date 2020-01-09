using System;
using TauCode.Parsing;
using TauCode.Parsing.Lexing;

namespace TauCode.TextProcessing.Lab.TextProcessors
{
    // todo clean up
    public class SkipLineBreaksProcessor : ITextProcessor<string>
    {
        public bool AcceptsFirstChar(char c) => LexingHelper.IsInlineWhiteSpace(c);

        public TextProcessingResult Process(TextProcessingContext context)
        {
            throw new NotImplementedException();
        }

        public string Produce(string text, int absoluteIndex, int consumedLength, Position position)
        {
            throw new NotImplementedException(); // todo should never be called
        }
    }
}
