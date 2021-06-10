using System.Reflection;

namespace TauCode.Lab.Xml.Bound
{
    internal class BoundAttributeDescriptor : IBoundAttributeDescriptor
    {
        #region ctor

        internal BoundAttributeDescriptor(string attributeName, PropertyInfo property, bool isMandatory)
        {
            this.AttributeName = attributeName;
            this.Property = property;
            this.IsMandatory = isMandatory;
        }

        #endregion

        #region IBoundAttributeDescriptor Members

        public string AttributeName { get; }
        public PropertyInfo Property { get; }
        public bool IsMandatory { get; }

        #endregion
    }
}