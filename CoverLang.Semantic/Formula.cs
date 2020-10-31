using System.Linq.Expressions;
using System.Runtime.InteropServices;

namespace CoverLang
{
    public class FormulaSignature
    {
        public string Name { get; set; }
        public CoverLangDataType ReturnDataType { get; set; }
    }
    public class Formula
    {
        public FormulaSignature Signature
        {
            get;
            set;
        }
        public Attribute[] Operands { get; set; }
        public CoverLangDataType ReturnDataType { get; set; }
        public string Body { get; set; }
    }

    public class FormulaOperand
    {
        public string Name { get; set; }
        public CoverLangDataType Type { get; set; }
    }
}