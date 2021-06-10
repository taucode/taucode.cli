using System;

namespace TauCode.Lab.Xml
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ElementAttribute : Attribute
    {
        public ElementAttribute(string elementName)
        {
            this.ElementName = elementName;
        }

        public string ElementName { get; }

        // todo: is mandatory
    }
}