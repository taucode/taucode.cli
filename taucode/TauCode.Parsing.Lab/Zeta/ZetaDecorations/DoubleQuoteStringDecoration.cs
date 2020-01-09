namespace TauCode.Parsing.Lab.Zeta.ZetaDecorations
{
    public sealed class DoubleQuoteStringDecoration : IZetaDecoration
    {
        public static DoubleQuoteStringDecoration Instance { get; } = new DoubleQuoteStringDecoration();

        private DoubleQuoteStringDecoration()
        {   
        }
    }
}
