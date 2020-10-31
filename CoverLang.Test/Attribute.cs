using Xunit.Sdk;

namespace CoverLang.Test
{
    public class Attribute:CoverLangNode
    {
        public AttributeDataType Type { get; set; }
        public bool IsRequired { get; set; }
    }

    public enum AttributeDataType
    {
        Date,
        Int,
        Bool,
        String
    }
}