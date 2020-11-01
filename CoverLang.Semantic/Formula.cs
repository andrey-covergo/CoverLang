using System;

namespace CoverLang
{
    public class FormulaPart
    {
        public string[] Lines { get; set; }
    }

    public class IfStatement : FormulaPart
    {
        
    }

    public class ValueExpression : FormulaPart
    {
        public Operand[] Operands { get; set; }
        
    }
    
    public class Return : FormulaPart
    {
        
    }

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

        private FormulaPart[] Parts { get; set; }

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

    public class Operand
    {
        public Operand(string name, CoverLangDataType type)
        {
            //Name = 
        }
        public string Name { get; set; }
        public CoverLangDataType Type { get; set; }
      //  public object GetValue();
    }
}