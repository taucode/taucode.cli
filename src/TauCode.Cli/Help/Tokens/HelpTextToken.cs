using System;
using TauCode.Parsing;
using TauCode.Parsing.Tokens;

namespace TauCode.Cli.Help.Tokens
{
    internal class HelpTextToken : TokenBase
    {
        public HelpTextToken(string text, Position position, int consumedLength)
            : base(position, consumedLength)
        {
            this.Text = text ?? throw new ArgumentNullException(nameof(text));
        }

        public string Text { get; }

        public override string ToString() => this.Text;
    }
}
