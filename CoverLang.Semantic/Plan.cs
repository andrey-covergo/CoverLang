namespace CoverLang
{
    public class Plan
    {
        public string Name { get; set; }
        public Attribute[] Attributes { get; set; }
        public Formula[] Formulas { get; }
    }
}