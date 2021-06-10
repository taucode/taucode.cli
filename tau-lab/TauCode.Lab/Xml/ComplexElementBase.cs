using System.Collections.Generic;
using System.Xml.Serialization;
using TauCode.Lab.Xml.Unbound;

namespace TauCode.Lab.Xml
{
    public abstract class ComplexElementBase : ElementBase, IComplexElement
    {
        private readonly UnboundChildrenList _unboundChildren;

        protected ComplexElementBase()
        {
            _unboundChildren = new UnboundChildrenList();
        }

        [XmlIgnore]
        public IList<IElement> UnboundChildren => _unboundChildren;
    }
}
