using System;
using System.Collections.Generic;
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
        public string Name { get; set; }
        public string[] RequiredOperands { get; set; }
        public CoverLangDataType ReturnDataType { get; set; }
        public string Body => string.Join(Environment.NewLine, BodyLines);
        public string[] BodyLines { get; set; }

        public object Execute(CalculationContext context)
        {
            throw new NotImplementedException();
        }
    }
    
    public class CalculationContext
    {
        public Operand[] Operands { get; set; }

        public object GetOperandValue(Operand op)
        {
            throw new NotImplementedException();
        }
    }

    public interface Operand
    {
        public string Name { get;}
        public CoverLangDataType Type { get;}
        public object GetValue();
    }
}