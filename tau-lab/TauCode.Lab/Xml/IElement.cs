using TauCode.Lab.Xml.Unbound;

namespace TauCode.Lab.Xml
{
    public interface IElement
    {
        IUnboundSchema UnboundSchema { get; }
        IAttributeCollection UnboundAttributes { get; }
    }
}
