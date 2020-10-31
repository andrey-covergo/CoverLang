using System.Collections.Generic;

namespace CoverLang.Test
{
    public class Plan
    {
        public string Name { get; set; }
        public List<Attribute> Attributes { get; }
        public List<Formula> Formulas { get; }
        public string Pricing { get; }
        public string Benefits { get; }
    }
}