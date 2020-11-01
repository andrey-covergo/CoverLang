using System;
using FluentAssertions;
using Sprache;
using Xunit;

namespace CoverLang.Test
{

    public class FormulaSignatureParseTests
    {
        [Fact]
        public void Given_formula_signature_only_When_Parse_Then_signature_is_correct()
        {
            var coverlang = @"formula 'core pricing eligibility' returning bool as";

            var formula = FormulaGrammar.FormulaDefinition.Parse(coverlang);

            formula.Name.Should().Be("core pricing eligibility");
            formula.ReturnDataType.Should().Be(CoverLangDataType.Bool);
        }
    }
}