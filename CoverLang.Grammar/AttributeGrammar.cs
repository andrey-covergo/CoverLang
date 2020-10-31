using Sprache;

namespace CoverLang
{
    public static class AttributeGrammar
    {
        public static class Parts
        {
            public static readonly string RequiredKeyword = "required";
            public static readonly string OptionalKeyword = "optional";
            public static readonly string IsRequiredPart = $"{RequiredKeyword} or {OptionalKeyword}";
            public const string AttributeKeyword = "attribute";
            public const string Name = "attribute name";
            public const string DataTypePart = "'attribute data type'";
            public const string WithKeyword = "'with' keyword'";
            public const string TypeKeyword = "'type' keyword'";
        }

        public static readonly Parser<Attribute> Attribute =
                from optional in (CoverLangGrammar.KeyWord(Parts.OptionalKeyword).Return(true)
                                  .Or(CoverLangGrammar.KeyWord(Parts.RequiredKeyword).Return(false)))
                                  .Named(Parts.IsRequiredPart)
                from attributeKeyword in CoverLangGrammar.KeyWord(Parts.AttributeKeyword).Token().Named(Parts.AttributeKeyword)
                from name in CoverLangGrammar.Identifier.Named(Parts.Name)
                from withKeyword in CoverLangGrammar.KeyWord("with").Token().Named(Parts.WithKeyword)
                from typeKeyword in CoverLangGrammar.KeyWord("type").Token().Named(Parts.TypeKeyword)
                from dataType in CoverLangGrammar.DataType.Named(Parts.DataTypePart)
                select new Attribute {IsRequired = !optional, Type = dataType, Name = name};
    }
}