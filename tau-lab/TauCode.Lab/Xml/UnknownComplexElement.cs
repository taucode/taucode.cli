using System.Collections.Generic;
using TauCode.Lab.Xml.Unbound;

namespace TauCode.Lab.Xml
{
    // todo: cannot be 'part' of bound or unbound schema
    public sealed class UnknownComplexElement : IComplexElement
    {
        private readonly AttributeCollection _attributes;
        private readonly UnboundChildrenList _children;

        public UnknownComplexElement(string elementName)
        {
            // todo checks

            this.ElementName = elementName;
            _attributes = new AttributeCollection();
            _children = new UnboundChildrenList();
        }

        public string ElementName { get; }

        public IUnboundSchema UnboundSchema => null;

        public IAttributeCollection UnboundAttributes => _attributes;
        public IList<IElement> UnboundChildren => _children;
    }
}
