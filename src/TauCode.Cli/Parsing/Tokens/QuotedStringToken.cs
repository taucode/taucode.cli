namespace TauCode.Cli.Parsing.Tokens
{
    public class QuotedStringToken : TokenBase
    {
        internal QuotedStringToken(string originalValue)
        {
            this.OriginalValue = originalValue;
            this.QuoteSymbol = originalValue[0];
            this.UnquotedValue = originalValue.Substring(1, originalValue.Length - 2);
        }

        public string OriginalValue { get; }
        public string UnquotedValue { get; }
        public char QuoteSymbol { get; }

        public override string ToString() => OriginalValue;
    }
}
