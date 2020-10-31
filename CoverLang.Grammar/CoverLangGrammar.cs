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

        public static readonly Parser<string> AttributeKeyword =
            from text in Parse.Letter.AtLeastOnce().Text().Token() where text.ToLower() == "attribute" select text;

        public static readonly Parser<Attribute> Attribute =
            from hasKeyWord in Token("has")
            from optional in Token("optional").Return(true).Or(Token("required").Return(false))
            from attributeKeyword in Token("attribute")
            from name in Identifier
            from dataType in DataType
            select new Attribute {IsRequired = !optional, Type = dataType, Name = name};


        public static readonly Parser<Plan> Plan =
            from planToken in PlanToken
            from name in Identifier
            //from lineEnd in Parse.LineEnd
            // from steppedNewLine in Parse.WhiteSpace.Repeat(4).Text()
            from attributes in Attribute.Many()
            select new Plan {Name = name, Attributes = attributes.ToArray()};

        public static Parser<string> Token(string token)
        {
            return from leading in Parse.WhiteSpace.Many()
                from open in Parse.Letter.AtLeastOnce().Text()
                from rest in Parse.LetterOrDigit.Many().Text()
                from trailing in Parse.WhiteSpace.Many()
                where (open + rest).ToLower() == token
                select open + rest;
        }
    }
}