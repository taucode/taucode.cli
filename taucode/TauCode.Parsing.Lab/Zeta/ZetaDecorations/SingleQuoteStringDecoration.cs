namespace TauCode.Parsing.Lab.Zeta.ZetaDecorations
{
    public sealed class SingleQuoteStringDecoration : IZetaDecoration
    {
        public static SingleQuoteStringDecoration Instance { get; } = new SingleQuoteStringDecoration();

        private SingleQuoteStringDecoration()
        {   
        }
    }
}
