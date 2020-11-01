using System.Linq;
using FluentAssertions;
using Sprache;
using Xunit;
using Xunit.Abstractions;

namespace CoverLang.Test
{
    public class FormulaBodyCaptureTests
    {
        private readonly ITestOutputHelper _output;

        public FormulaBodyCaptureTests(ITestOutputHelper output)
        {
            _output = output;
        }
        [Fact]
        public void Given_formula_without_body_When_Parse_Then_receive_error()
        {
            var coverlang = @"formula 'core pricing eligibility' returning bool as";

            FormulaGrammar.Formula.Invoking(f =>ParserExtensions.Parse<Formula>(f, coverlang))
                .Should().Throw<ParseException>().WithMessageContaining(FormulaGrammar.Parts.Body);
        }
        
        [Fact]
        public void Given_formula_without_body_due_to_bad_indent_When_Parse_Then_receive_error()
        {
            var coverlang = @"
formula 'core pricing eligibility' returning bool as
return int";

            FormulaGrammar.Formula.Invoking(f =>f.Parse(coverlang.Trim()))
                .Should().Throw<ParseException>().WithMessageContaining(FormulaGrammar.Parts.Body);
        }
        
        
        [Fact]
        public void Given_formula_single_line_body_When_Parse_Then_can_execute_it()
        {
            var coverlang = @"
formula 'core pricing eligibility' returning bool as
    return true";

            var formula = FormulaGrammar.Formula.Parse(coverlang.Trim());

            formula.Name.Should().Be("core pricing eligibility");
            formula.ReturnDataType.Should().Be(CoverLangDataType.Bool);
            formula.BodyLines.Should().HaveCount(1);
            formula.BodyLines.Should().NotContain("");
            formula.Body.Should().Be("return true");
        }
 

        [Fact]
        public void Given_Formula_with_nested_body_When_Parse_Then_can_execute_it()
        {
            var coverlang = @"
formula 'core pricing eligibility' returning bool as 
    if 'num of employees covered' > 10 or AreAllEmployeesCovered then
        if 'end date' > '10 Nov 2020' return true
        return false
    return true";
            var formula = FormulaGrammar.Formula.Parse(coverlang.Trim());
            formula.Name.Should().Be("core pricing eligibility");
            formula.ReturnDataType.Should().Be(CoverLangDataType.Bool);

            _output.WriteLine(formula.BodyLines.First());
            
            formula.BodyLines.Should().BeEquivalentTo(
                "if 'num of employees covered' > 10 or AreAllEmployeesCovered then",
                "    if 'end date' > '10 Nov 2020' return true",
                "    return false",
                "return true");
            
            formula.Body.Should().Be(@"
if 'num of employees covered' > 10 or AreAllEmployeesCovered then
    if 'end date' > '10 Nov 2020' return true
    return false
return true".Trim());
        }
    }
}