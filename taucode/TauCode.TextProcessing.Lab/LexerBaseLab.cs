using System;
using System.Collections.Generic;
using TauCode.Parsing;
using TauCode.Parsing.Lexing;

namespace TauCode.TextProcessing.Lab
{
    public abstract class LexerBaseLab : ILexer
    {
        private TextProcessingContext _context;

        private IList<IGammaTokenExtractor> _tokenExtractors;
        private IList<ICharProcessor> _charSkippers;

        protected IList<IGammaTokenExtractor> TokenExtractors =>
            _tokenExtractors ?? (_tokenExtractors = this.CreateTokenExtractors());

        protected IList<ICharProcessor> CharSkippers =>
            _charSkippers ?? (_charSkippers = this.CreateCharSkippers());

        protected abstract IList<IGammaTokenExtractor> CreateTokenExtractors();
        protected abstract IList<ICharProcessor> CreateCharSkippers();

        public IList<IToken> Lexize(string input)
        {
            // todo check args
            var tokens = new List<IToken>();
            _context = new TextProcessingContext(input);

            while (true)
            {
                if (_context.IsEnd())
                {
                    break;
                }

                var gotSuccess = false;

                foreach (var tokenExtractor in this.TokenExtractors)
                {
                    if (gotSuccess)
                    {
                        break;
                    }

                    var result = tokenExtractor.Process(_context);
                    if (_context.Depth != 1)
                    {
                        throw new NotImplementedException();
                    }

                    switch (result.Summary)
                    {
                        case TextProcessingSummary.Skip:
                            gotSuccess = true;
                            _context.Advance(result.IndexShift, result.LineShift, result.GetCurrentColumn());
                            break;

                        case TextProcessingSummary.CanProduce:
                            var token = tokenExtractor.Produce(
                                _context.Text,
                                _context.GetStartingIndex(),
                                result.IndexShift);

                            if (token == null)
                            {
                                // No luck. It can happen - for example, Lisp Symbol extractor will accept every char of "1111", but will refuse produce a whole result,
                                // because "1111" should be lexized as an Integer, not a Symbol.
                            }
                            else
                            {
                                gotSuccess = true;
                                _context.Advance(result.IndexShift, result.LineShift, result.GetCurrentColumn());

                                if (token.HasPayload)
                                {
                                    tokens.Add(token);
                                }
                            }

                            break;

                        case TextProcessingSummary.Fail: // no luck for this extractor
                            break;

                        default:
                            throw new NotImplementedException();
                    }
                }

                if (!gotSuccess)
                {
                    var charRecognized = false;

                    foreach (var charSkipper in this.CharSkippers)
                    {
                        if (charRecognized)
                        {
                            break;
                        }

                        var result = charSkipper.ProcessChar(_context.GetCurrentChar());

                        switch (result)
                        {
                            case CharProcessingResult.Skip:
                                //_context.Advance(1, 0, _context.GetCurrentColumn() + 1);
                                _context.AdvanceByChar();
                                charRecognized = true;
                                break;

                            case CharProcessingResult.Use:
                                throw new NotImplementedException(); // should not happen for skipper.

                            case CharProcessingResult.Fail:
                                // try another
                                break;

                            default:
                                throw new ArgumentOutOfRangeException();
                        }

                        if (result == CharProcessingResult.Skip)
                        {
                            break;
                        }
                    }

                    if (!charRecognized)
                    {
                        throw new NotImplementedException();
                    }
                }
            }

            return tokens;
        }
    }
}
