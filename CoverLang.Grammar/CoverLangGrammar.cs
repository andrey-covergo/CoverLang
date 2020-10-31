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
            public static class Parts
            {
                public const string HasKeyword = "'has' keyword";
                public const string RequiredKeyword = "'require' keyword";
                public const string AttributePart = "'attribute' keyword";
                public const string NamePart = "attribute name";
                public const string DataTypePart = "'attribute data type'";
            }
        }
        
        public static readonly Parser<Attribute> Attribute =
            (from hasKeyWord in Token("has").Named(AttributeGrammar.Parts.HasKeyword)
                from optional in (Token("optional").Return(true).Or(Token("required").Return(false))).Named(AttributeGrammar.Parts.RequiredKeyword)
                from attributeKeyword in Token("attribute").Named(AttributeGrammar.Parts.AttributePart)
                from name in Identifier.Named(AttributeGrammar.Parts.NamePart)
                from dataType in DataType.Named(AttributeGrammar.Parts.DataTypePart)
                select new Attribute {IsRequired = !optional, Type = dataType, Name = name});

        public static class PlanGrammar
        {
            public static class Parts
            {
                public const string PlanKeyword = "'Plan' keyword";
                public const string Name = "plan name";
            }
        }
        
        public static readonly Parser<Plan> Plan =
            from planToken in Token("plan").Named(PlanGrammar.Parts.PlanKeyword)
            from name in Identifier.Named(PlanGrammar.Parts.Name)
            from attributes in Attribute.XMany()
            select new Plan {Name = name, Attributes = attributes.ToArray()};

        public static class TokenGrammar
        {
            public static string TokenKeyword(string token) => $"token '{token}' keyword";
        }
        public static Parser<string> Token(string token)
        {
            return 
                (from leading in Parse.WhiteSpace.Many().Text().Many().Or(Parse.LineEnd.Many())
                from open in Parse.Letter.AtLeastOnce().Text()
                from rest in Parse.LetterOrDigit.Many().Text()
                from trailing in Parse.WhiteSpace.Many()
                select open + rest).Where(p => p.ToLower() == token).Named(TokenGrammar.TokenKeyword(token));
        }
    }
}