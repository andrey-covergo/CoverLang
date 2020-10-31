using System.Linq;
using Sprache;

namespace CoverLang
{
    public static class PlanGrammar
    {
        public static class Parts
        {
            public const string PlanKeyword = "'Plan' keyword";
            public const string Name = "plan name";
        }

        public static readonly Parser<Plan> Plan =
            from planToken in CoverLangGrammar.KeyWord("Plan").Token().Named(Parts.PlanKeyword)
            from name in CoverLangGrammar.Identifier.Named(Parts.Name)
            from attributes in CoverLangGrammar.NestedIndent.Then(i=>AttributeGrammar.Attribute).Many()
            from emptySpace in Parse.WhiteSpace.Many().Text().Or(Parse.LineEnd).Many() 
            from eol in Parse.LineTerminator
            select new Plan {Name = name, Attributes = attributes.ToArray()};


    }
}