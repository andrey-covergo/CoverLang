using System.Collections.Generic;

namespace CoverLang.Test
{
    public class Plan
    {
        public string Name { get; set; }
        public Attribute[] Attributes { get; set; }
        public Formula[] Formulas { get; }
        public string Pricing { get; }
        public string Benefits { get; }
    }
}