namespace TauCode.Parsing.Lab.Exceptions
{
    public class UnexpectedEndOfClauseException : ParseClauseFailedException
    {
        public UnexpectedEndOfClauseException(object[] partialParsingResults)
            : base("Unexpected end of clause.", partialParsingResults)
        {
        }
    }
}
