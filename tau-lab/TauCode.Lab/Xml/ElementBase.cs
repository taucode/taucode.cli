using System.Xml.Serialization;
using TauCode.Lab.Xml.Unbound;

namespace TauCode.Lab.Xml
{
    public abstract class ElementBase : IElement
    {
        private readonly AttributeCollection _unboundAttributes;

        protected ElementBase()
        {
            _unboundAttributes = new AttributeCollection();
        }

        [XmlIgnore]
        public virtual IUnboundSchema UnboundSchema => null;

        [XmlIgnore]
        public IAttributeCollection UnboundAttributes => _unboundAttributes;
    }
}
