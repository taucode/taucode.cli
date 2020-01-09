using System;
using TauCode.Parsing;

namespace TauCode.TextProcessing.Lab
{
    public abstract class GammaTokenExtractorBase<TToken> : IGammaTokenExtractor<TToken>
        where TToken : IToken
    {
        protected TextProcessingContext Context { get; private set; }

        public abstract TToken ProduceToken(string text, int startingIndex, int length);

        protected virtual TextProcessingResult Delegate()
        {
            return TextProcessingResult.Fail;
        }

        public TextProcessingResult Process(TextProcessingContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var previousChar = context.GetPreviousChar();
            if (previousChar.HasValue)
            {
                var acceptsChar = this.AcceptsPreviousCharImpl(previousChar.Value);
                if (!acceptsChar)
                {
                    return TextProcessingResult.Fail;
                }
            }

            this.Context = context;
            this.Context.RequestGeneration();

            while (true)
            {
                if (this.Context.IsEnd())
                {
                    throw new NotImplementedException();
                }

                var delegatedResult = this.Delegate();
                if (delegatedResult.Summary != TextProcessingSummary.Fail)
                {
                    throw new NotImplementedException();
                }

                var c = this.Context.GetCurrentChar();
                var localIndex = this.Context.GetLocalIndex();

                if (this.AcceptsCharImpl(c, localIndex))
                {
                    this.Context.AdvanceByChar();
                }
                else
                {
                    throw new NotImplementedException();
                }

                throw new NotImplementedException();
            }

            throw new System.NotImplementedException();
        }

        protected abstract bool AcceptsPreviousCharImpl(char previousChar);

        protected abstract bool AcceptsCharImpl(char c, int localIndex);

        public IToken Produce(string text, int startingIndex, int length) =>
            this.ProduceToken(text, startingIndex, length);
    }
}
