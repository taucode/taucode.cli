using System;

namespace TauCode.Lab.Xml
{
    // todo: used for _BOTH_ serialization and deserialization of simple values (attributes and text node elements).
    [AttributeUsage(AttributeTargets.Property)]
    public class XmlValueConverterAttribute : Attribute
    {
        public XmlValueConverterAttribute(Type converterType)
        {
            // todo checks

            this.ConverterType = converterType;
        }

        public Type ConverterType { get; }
    }
}
