namespace CoverLang
{
    public class Attribute:CoverLangNode
    {
        public AttributeDataType Type { get; set; }
        public bool IsRequired { get; set; }
    }
}