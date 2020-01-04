using TauCode.Parsing.Exceptions;

namespace TauCode.Parsing.Lab.Exceptions
{
    public class InternalParsingLogicException : ParsingException
    {
        public InternalParsingLogicException(string message)
            : base(message)
        {
        }
    }
}
