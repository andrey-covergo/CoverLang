using System;
using FluentAssertions;
using Sprache;
using Xunit;

namespace CoverLang.Test
{
    public class FormulaParseTests
    {
        
        [Fact]
        public void Given_formula_without_body_When_Parse_Then_receive_error()
        {
            var coverlang = @"Formula 'core pricing eligibility' returning bool as";

             FormulaGrammar.Formula.Invoking(f =>f.Parse(coverlang))
                .Should().Throw<ParseException>().WithMessageContaining(FormulaGrammar.Parts.Body);
        }
        
        [Fact]
        public void Given_formula_signature_only_When_Parse_Then_signature_is_correct()
        {
            var coverlang = @"
Formula 'core pricing eligibility' returning bool as";

            var formula = FormulaGrammar.FormulaDefinition.Parse(coverlang);

            formula.Name.Should().Be("core pricing eligibility");
            formula.ReturnDataType.Should().Be(CoverLangDataType.Bool);
        }
        
        [Fact]
        public void Given_formula_only_returning_value_When_Parse_Then_can_execute_it()
        {
            var coverlang = @"
Formula 'core pricing eligibility' returning bool as 
    return true";

            var formula = FormulaGrammar.Formula.Parse(coverlang);

            formula.Signature.Name.Should().Be("core pricing eligibility");
            formula.ReturnDataType.Should().Be(CoverLangDataType.Bool);
            formula.Body.Should().Be("return true");
        }
        
        [Fact]
        public void Given_formula_with_if_body_value_When_Parse_Then_can_execute_it()
        {
            var coverlang = @"
Formula 'core pricing eligibility' returning bool as 
    if true then return true";
            var formula = FormulaGrammar.Formula.Parse(coverlang);

            formula.Signature.Should().Be("core pricing eligibility");
            formula.ReturnDataType.Should().Be(CoverLangDataType.Bool);
            formula.Body.Should().Be("if true then return true");
        }
        
        [Fact]
        public void Given_formula_with_if_body_value_going_else_path_When_Parse_Then_can_execute_it()
        {
            var coverlang = @"
Formula 'core pricing eligibility' returning int as 
    if false then return 1";
            var formula = FormulaGrammar.Formula.Parse(coverlang);
            formula.Signature.Should().Be("core pricing eligibility");
            formula.ReturnDataType.Should().Be(CoverLangDataType.Int);
            formula.Body.Should().Be(" if false then return 1");
        }
        
        [Fact]
        public void Given_formula_capturing_attribute_When_Parse_Then_can_execute_it()
        {
            var coverlang = @"
Formula 'core pricing eligibility' returning int as
      if 'total number of employees' > 1 then return 'total number of employees'- 5";
            var formula = FormulaGrammar.Formula.Parse(coverlang);
            formula.Signature.Should().Be("core pricing eligibility");
            formula.ReturnDataType.Should().Be(CoverLangDataType.Int);
            formula.Body.Should().Be("if 'total number of employees' > 1 then return 'total number of employees'- 5");
        }

        [Fact]
        public void Given_Formula_with_nested_body_When_Parse_Then_can_execute_it()
        {
            var coverlang = @"
Formula 'core pricing eligibility' returning bool as 
    if 'num of employees covered' > 10 or AreAllEmployeesCovered then
        if 'end date' > '10 Nov 2020' return true
        return false
    return true
        ";
            var formula = FormulaGrammar.Formula.Parse(coverlang);
            formula.Signature.Should().Be("core pricing eligibility");
            formula.ReturnDataType.Should().Be(CoverLangDataType.Int);
            formula.Body.Should().Be(@"
if 'num of employees covered' > 10 or AreAllEmployeesCovered then
    if 'end date' > '10 Nov 2020' return true
    return false
return true");
        }
    }
}