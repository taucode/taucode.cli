using System;
using System.Collections.Generic;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Lab.Zeta.Tokens
{
    public class ZetaToken : TokenBase
    {
        public ZetaToken(
            string text,
            IZetaClass @class,
            IZetaDecoration decoration,
            Position position,
            int consumedLength,
            string name = null,
            IEnumerable<KeyValuePair<string, string>> properties = null)
            : base(position, consumedLength, name, properties)
        {
            this.Text = text ?? throw new ArgumentNullException(nameof(text));
            this.Class = @class ?? throw new ArgumentNullException(nameof(@class));
            this.Decoration = decoration ?? throw new ArgumentNullException(nameof(decoration));
        }

        public string Text { get; }
        public IZetaClass Class { get; }
        public IZetaDecoration Decoration { get; }
    }
}
