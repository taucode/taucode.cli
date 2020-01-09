﻿using TauCode.Parsing;

namespace TauCode.TextProcessing.Lab
{
    public interface IGammaTokenExtractor<out TToken> : IGammaTokenExtractor
        where TToken : IToken
    {
        TToken ProduceToken(string text, int startingIndex, int length);
    }
}