using Sprache;

namespace CoverLang.Test
{
    public class CoverLangGrammar
    {
        public static readonly Parser<string> MonoIdentifier = 
            from open in Parse.Letter.AtLeastOnce().Text()
            from rest in Parse.LetterOrDigit.Or(Parse.Char('_')).Or(Parse.Char('-')).Many().Text()
            select open+rest;

        public static Parser<string> Token(string token) => 
            from leading in Parse.WhiteSpace.Many()
            from open in Parse.Letter.AtLeastOnce().Text()
            from rest in Parse.LetterOrDigit.Many().Text()
            from trailing in Parse.WhiteSpace.Many()
            where (open+rest).ToLower() == token
            select open+rest;
        
        public static readonly Parser<string> PlanIdentifier =
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
        
        public static readonly Parser<string> HasKeyword =
            from text in Parse.Letter.AtLeastOnce().Text() where text.ToLower() == "has" select text;

        public static readonly Parser<AttributeDataType> DataType =
            from withKeyword in Token("with")
            from typeKeyword in Token("type")
            from typeData in Parse.Chars("int").Return(AttributeDataType.Int)
                .Or(Parse.Chars("bool").Return(AttributeDataType.Bool))
                .Or(Parse.Chars("string").Return(AttributeDataType.String))
                .Or(Parse.Chars("date").Return(AttributeDataType.Date))
            select typeData;
       
        public static readonly Parser<string> AttributeKeyword =
            from text in Parse.Letter.AtLeastOnce().Text().Token() where text.ToLower() == "attribute" select text;

        public static readonly Parser<Attribute> Attribute =
            from hasKeyWord in Token("has")
            from optional in Token("optional").Return(true).Or(Token("required").Return(false))
            from attributeKeyword in AttributeKeyword
            from name in Identifier
            from dataType in DataType
            select new Attribute() {IsRequired = !optional, Type = dataType, Name = name};
             
            
        public static readonly Parser<Plan> Plan =
            from planToken in PlanIdentifier
            from name in Identifier
            select new Plan{Name = name};
    }
}