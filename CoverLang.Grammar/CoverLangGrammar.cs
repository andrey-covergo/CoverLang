using System.Linq;
using Sprache;

namespace CoverLang
{
    public class CoverLangGrammar
    {
        public static readonly Parser<string> MonoIdentifier =
            from open in Parse.Letter.AtLeastOnce().Text()
            from rest in Parse.LetterOrDigit.Or(Parse.Char('_')).Or(Parse.Char('-')).Many().Text()
            select open + rest;

        public static readonly Parser<string> PlanToken =
            from text in Parse.Letter.AtLeastOnce().Text().Token() where text == "Plan" select text;

        public static readonly Parser<string> QuotedText =
            (from open in Parse.Char('"')
                from content in Parse.CharExcept('"').Many().Text()
                from close in Parse.Char('"')
                select content).Token();

        public static readonly Parser<string> SingleQuotedText =
            (from open in Parse.Char('\'')
                from content in Parse.CharExcept('\'').Many().Text()
                from close in Parse.Char('\'')
                select content).Token();

        public static readonly Parser<string> Identifier =
            from token in MonoIdentifier.Or(SingleQuotedText).Or(QuotedText)
            select token;

        public static readonly Parser<AttributeDataType> DataType =
            from withKeyword in Token("with")
            from typeKeyword in Token("type")
            from type in Token("int").Return(AttributeDataType.Int)
                .Or(Token("bool").Return(AttributeDataType.Bool))
                .Or(Token("string").Return(AttributeDataType.String))
                .Or(Token("date").Return(AttributeDataType.Date))
            select type;



        public static class AttributeGrammar
        {
            public const string HasKeywordSection = "'has keyword section'";
            public const string RequiredSection = "'require section'";
            public const string AttributeSection = "'attribute section'";
            public const string NameSection = "'attribute name section'";
            public const string DataTypeSection = "'attribute data type section'";
        }
        
        public static readonly Parser<Attribute> Attribute =
            (from hasKeyWord in Token("has").Named(AttributeGrammar.HasKeywordSection)
                from optional in (Token("optional").Return(true).Or(Token("required").Return(false))).Named(AttributeGrammar.RequiredSection)
                from attributeKeyword in Token("attribute").Named(AttributeGrammar.AttributeSection)
                from name in Identifier.Named(AttributeGrammar.NameSection)
                from dataType in DataType.Named(AttributeGrammar.DataTypeSection)
                select new Attribute {IsRequired = !optional, Type = dataType, Name = name});

        public static readonly Parser<Plan> Plan =
            (from planToken in Token("plan")
            from name in Identifier.Named("plan name")
            from attributes in Attribute.Many().Named("plan attributes")
            select new Plan {Name = name, Attributes = attributes.ToArray()});

        public static Parser<string> Token(string token)
        {
            return 
                (from leading in Parse.WhiteSpace.Many().Text().Many().Or(Parse.LineEnd.Many())
                from open in Parse.Letter.AtLeastOnce().Text()
                from rest in Parse.LetterOrDigit.Many().Text()
                from trailing in Parse.WhiteSpace.Many()
                select open + rest).Where(p => p.ToLower() == token).Named("token "+token);
        }
    }
}