using Sprache;

namespace CoverLang
{
    public class CoverLangGrammar
    {
        public static readonly Parser<string> MonoIdentifier =
            from open in Parse.Letter.AtLeastOnce().Text()
            from rest in Parse.LetterOrDigit.Or(Parse.Char('_')).Or(Parse.Char('-')).Many().Text()
            select open + rest;
        
        public static readonly Parser<string> QuotedText =
                from open in Parse.Char('"')
                from content in Parse.CharExcept('"').Many().Text().Token()
                from close in Parse.Char('"')
                select content;

        public static readonly Parser<string> SingleQuotedText =
                from open in Parse.Char('\'')
                from content in Parse.CharExcept('\'').Many().Text().Token()
                from close in Parse.Char('\'')
                select content;

        public static readonly Parser<string> Identifier =
            from token in MonoIdentifier.XOr(SingleQuotedText).XOr(QuotedText)
            select token;


        public static readonly Parser<string> Indent = Parse.WhiteSpace.Repeat(4).Text();
        public static readonly Parser<string> EmptyLineEnd = Parse.WhiteSpace.Until(Parse.LineEnd).Text();

        
        public static readonly Parser<string> NestedIndent = 
             from newLine in EmptyLineEnd
             from indent in Indent
             select indent;
            
        public static readonly Parser<CoverLangDataType> DataType =
     
            from type in KeyWord("int").Return(CoverLangDataType.Int)
                                .Or(KeyWord("bool").Return(CoverLangDataType.Bool))
                                .Or(KeyWord("string").Return(CoverLangDataType.String))
                                .Or(KeyWord("date").Return(CoverLangDataType.Date))
            select type;


        public static class KeywordGrammar
        {
            public static string Keyword(string token) => $"token '{token}' keyword";
        }

        public static Parser<char> OpenBracket = Parse.Char('{').Token();

        public static Parser<char> CloseBracket = Parse.Char('}').Token();

        public static Parser<string> KeyWord(string token) =>Parse.String(token).Text().Named(KeywordGrammar.Keyword(token));
    }
}