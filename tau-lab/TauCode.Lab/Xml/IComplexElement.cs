using System.Collections.Generic;

namespace TauCode.Lab.Xml
{
    public interface IComplexElement : IElement
    {
        IList<IElement> UnboundChildren { get; }
    }
}
