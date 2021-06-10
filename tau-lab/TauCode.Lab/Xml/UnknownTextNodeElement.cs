using System;
using TauCode.Lab.Xml.Unbound;

namespace TauCode.Lab.Xml
{
    // todo: cannot be 'part' of bound or unbound schema
    public sealed class UnknownTextNodeElement : ITextNodeElement
    {
        public UnknownTextNodeElement(string elementName)
        {
            // todo checks

            this.ElementName = elementName;
        }

        public string ElementName { get; }

        public IUnboundSchema UnboundSchema => null;

        public IAttributeCollection UnboundAttributes => throw new NotImplementedException();

        public string Value
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }
    }
}
