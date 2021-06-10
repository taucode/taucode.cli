using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TauCode.Lab.Xml.Bound
{
    internal class BoundSchemaBase : IBoundSchema
    {
        #region Fields

        private readonly Dictionary<string, IBoundAttributeDescriptor> _boundAttributeDescriptors;

        private readonly List<IBoundChildElementDescriptor> _allBoundChildElementDescriptors;
        private readonly Dictionary<string, IBoundChildElementDescriptor> _boundTextNodeElementValueDescriptors;
        private readonly Dictionary<string, IBoundChildElementDescriptor> _boundElementDescriptors;

        private List<string> _cachedMandatoryAttributeNames;

        #endregion

        #region ctor

        internal BoundSchemaBase()
        {
            _boundAttributeDescriptors = new Dictionary<string, IBoundAttributeDescriptor>();

            _allBoundChildElementDescriptors = new List<IBoundChildElementDescriptor>();
            _boundTextNodeElementValueDescriptors = new Dictionary<string, IBoundChildElementDescriptor>();
            _boundElementDescriptors = new Dictionary<string, IBoundChildElementDescriptor>();
        }

        #endregion

        #region Private

        private List<string> GetMandatoryAttributeNames() => _boundAttributeDescriptors
            .Select(x => x.Value)
            .Where(x => x.IsMandatory)
            .Select(x => x.AttributeName)
            .ToList();

        #endregion

        #region Internal

        internal IReadOnlyList<string> MandatoryAttributeNames =>
            _cachedMandatoryAttributeNames ??= this.GetMandatoryAttributeNames();

        internal void AddBoundAttribute(string attributeName, PropertyInfo property, bool isMandatory)
        {
            // todo: check name doesn't exist already

            var boundAttributeDescriptor = new BoundAttributeDescriptor(attributeName, property, isMandatory);
            _boundAttributeDescriptors.Add(boundAttributeDescriptor.AttributeName, boundAttributeDescriptor);

            _cachedMandatoryAttributeNames = null;
        }

        internal void AddBoundTextNodeElementValue(string elementName, PropertyInfo property, bool isMandatory)
        {
            // todo: check name doesn't exist already (in two element-related dictionaries)

            int minOccurrence;
            int maxOccurrence;

            if (isMandatory)
            {
                minOccurrence = 1;
                maxOccurrence = 1;
            }
            else
            {
                minOccurrence = 0;
                maxOccurrence = 1;
            }

            var boundElementDescriptor = new BoundChildElementDescriptor(elementName, property, minOccurrence, maxOccurrence);
            _boundTextNodeElementValueDescriptors.Add(boundElementDescriptor.ElementName, boundElementDescriptor);

            _allBoundChildElementDescriptors.Add(boundElementDescriptor);
        }

        internal void AddBoundElement(string elementName, PropertyInfo property, int minOccurrence, int? maxOccurrence)
        {
            // todo: check name doesn't exist already (in two element-related dictionaries)

            var boundElementDescriptor = new BoundChildElementDescriptor(elementName, property, minOccurrence, maxOccurrence);
            _boundElementDescriptors.Add(boundElementDescriptor.ElementName, boundElementDescriptor);

            _allBoundChildElementDescriptors.Add(boundElementDescriptor);
        }

        #endregion

        #region IBoundSchema Members

        public IReadOnlyDictionary<string, IBoundAttributeDescriptor> BoundAttributeDescriptors =>
            _boundAttributeDescriptors;

        public IReadOnlyList<IBoundChildElementDescriptor> AllBoundChildElementDescriptors => _allBoundChildElementDescriptors;

        public IReadOnlyDictionary<string, IBoundChildElementDescriptor> BoundChildTextNodeElementValueDescriptors =>
            _boundTextNodeElementValueDescriptors;

        public IReadOnlyDictionary<string, IBoundChildElementDescriptor> BoundChildElementDescriptors =>
            _boundElementDescriptors;

        #endregion
    }
}
