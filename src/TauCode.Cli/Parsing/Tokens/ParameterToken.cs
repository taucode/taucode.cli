namespace TauCode.Cli.Parsing.Tokens
{
    public class ParameterToken : TokenBase
    {
        internal ParameterToken(string parameterAlias)
        {
            this.ParameterAlias = parameterAlias;
        }

        public string ParameterAlias { get; }

        public override string ToString() => ParameterAlias;
    }
}
