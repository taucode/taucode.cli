using System.Collections.Generic;
using System.Linq;

namespace TauCode.Lab.Xml.Unbound
{
    public class UnboundSchemaBase : IUnboundSchema
    {
        #region Static

        public static IUnboundSchema RestrictingEmptyUnboundSchema { get; } = new UnboundSchemaBase(
            new Dictionary<string, bool>(),
            false,
            new List<UnboundChildElementDescriptor>(),
            false);

        public static IUnboundSchema PermissiveEmptyUnboundSchema { get; } = new UnboundSchemaBase(
            new Dictionary<string, bool>(),
            true,
            new List<UnboundChildElementDescriptor>(),
            true);

        #endregion

        #region Fields

        private readonly Dictionary<string, bool> _unboundAttributes;

        private List<string> _cachedMandatoryAttributeNames;
        private Dictionary<string, UnboundChildElementDescriptor> _unboundChildElementDescriptors;


        #endregion

        #region ctor

        public UnboundSchemaBase(
            IReadOnlyDictionary<string, bool> unboundAttributes,
            bool allowsUnknownAttributes,
            IEnumerable<UnboundChildElementDescriptor> unboundChildElementDescriptors,
            bool allowsUnknownChildElements)
        {
            // todo checks

            _unboundAttributes = new Dictionary<string, bool>(unboundAttributes);
            this.AllowsUnknownAttributes = allowsUnknownAttributes;
            this.UnboundChildElementDescriptors = unboundChildElementDescriptors.ToList();
            this.AllowsUnknownChildElements = allowsUnknownChildElements;
        }

        internal UnboundSchemaBase(IUnboundSchema origin)
            : this(
                origin.UnboundAttributes,
                origin.AllowsUnknownAttributes,
                origin.UnboundChildElementDescriptors,
                origin.AllowsUnknownChildElements)
        {
        }

        #endregion

        #region Private

        private List<string> GetMandatoryAttributeNames() => _unboundAttributes
            .Where(x => x.Value)
            .Select(x => x.Key)
            .ToList();

        private Dictionary<string, UnboundChildElementDescriptor> GetUnboundChildElementDescriptorByElementName() =>
            this.UnboundChildElementDescriptors
                .ToDictionary(x => x.ElementName, x => x);

        #endregion

        #region Internal

        internal IReadOnlyList<string> MandatoryAttributeNames =>
            _cachedMandatoryAttributeNames ??= this.GetMandatoryAttributeNames();

        internal IReadOnlyDictionary<string, UnboundChildElementDescriptor>
            UnboundChildElementDescriptorByElementName =>
            _unboundChildElementDescriptors ??= this.GetUnboundChildElementDescriptorByElementName();

        #endregion

        #region IUnboundSchema Members

        public IReadOnlyDictionary<string, bool> UnboundAttributes => _unboundAttributes;
        public bool AllowsUnknownAttributes { get; }
        public IReadOnlyList<UnboundChildElementDescriptor> UnboundChildElementDescriptors { get; }
        public bool AllowsUnknownChildElements { get; }

        #endregion
    }
}
