namespace TauCode.Lab.Xml
{
    public class SerializationSettings
    {
        // todo: camel-case switch

        public static SerializationSettings CreateDefault()
        {
            var settings = new SerializationSettings
            {
                BoundPropertyValueConverter = new DefaultBoundPropertyValueConverter(),
            };

            return settings;
        }

        public IBoundPropertyValueConverter BoundPropertyValueConverter { get; set; }
    }
}
