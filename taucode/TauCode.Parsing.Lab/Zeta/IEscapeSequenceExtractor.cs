using System.Collections.Generic;

namespace TauCode.Parsing.Lab.Zeta
{
    public interface IEscapeSequenceExtractor
    {
        IList<char> StartingChars { get; }
        IList<char> EscapeChars { get; }
    }
}
