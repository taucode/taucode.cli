using System.Reflection;

namespace TauCode.Lab.Xml.Bound
{
    internal class BoundChildElementDescriptor : IBoundChildElementDescriptor
    {
        #region ctor

        internal BoundChildElementDescriptor(
            string elementName,
            PropertyInfo property,
            int minOccurrence,
            int? maxOccurrence)
        {
            this.ElementName = elementName;
            this.Property = property;
            this.MinOccurrence = minOccurrence;
            this.MaxOccurrence = maxOccurrence;
        }

        #endregion

        #region IBoundElementDescriptor Members

        public string ElementName { get; }
        public int MinOccurrence { get; }
        public int? MaxOccurrence { get; }
        public PropertyInfo Property { get; }

        #endregion
    }
}