using System.Collections.Generic;

namespace TauCode.Lab.Xml.Bound
{
    internal interface IBoundSchema
    {
        IReadOnlyDictionary<string, IBoundAttributeDescriptor> BoundAttributeDescriptors { get; }
        IReadOnlyList<IBoundChildElementDescriptor> AllBoundChildElementDescriptors { get; }
        IReadOnlyDictionary<string, IBoundChildElementDescriptor> BoundChildTextNodeElementValueDescriptors { get; }
        IReadOnlyDictionary<string, IBoundChildElementDescriptor> BoundChildElementDescriptors { get; }
    }
}
