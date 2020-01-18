using System;
using TauCode.Parsing.Exceptions;

namespace TauCode.Parsing.Lab.Exceptions
{
    // todo: get rid of.
    [Serializable]
    public class UnexpectedTokenExceptionLab : ParseClauseFailedException
    {
        public UnexpectedTokenExceptionLab(IToken token, object[] partialParsingResults)
            : base($"Unexpected token: '{token}'.", partialParsingResults)
        {
        }
    }
}
