using System;
using TauCode.Lab.Xml.Bound;
using TauCode.Lab.Xml.Unbound;

namespace TauCode.Lab.Xml
{
    internal interface IElementTypeDescriptor
    {
        Type ElementType { get; }
        IBoundSchema BoundSchema { get; }
        IUnboundSchema UnboundSchema { get; }
    }
}
