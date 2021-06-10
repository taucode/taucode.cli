namespace TauCode.Lab.Xml
{
    public interface IDocument
    {
        public Declaration Declaration { get; set; }
        string Xmlns { get; set; }
        string RootElementName { get; }
    }
}
