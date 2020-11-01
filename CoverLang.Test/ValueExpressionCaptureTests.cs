using FluentAssertions;
using Sprache;
using Xunit;
using Xunit.Abstractions;

namespace CoverLang.Test
{
    
      public class ValueExpressionConstantsCaptureTests
    {
        private readonly ITestOutputHelper _output;

        public ValueExpressionConstantsCaptureTests(ITestOutputHelper output)
        {
            _output = output;
        }
        
        [Fact]
        public void Given_constant_date_literal_When_Parse_Then_it_is_success()
        {
            var coverlang = @"'10 Nov 2020'";

            var expression = FormulaGrammar.ValueExpressionStatement.Parse(coverlang);
            expression.Operands.Should().BeEquivalentTo(new Operand(){Name = "10 Nov 2020", Type = CoverLangDataType.Date});
            expression.Lines.Should().BeEquivalentTo("'10 Nov 2020'");
        }
        
        [Fact]
        public void Given_constant_int_literal_When_Parse_Then_it_is_success()
        {
            var coverlang = @"2020";

            var expression = FormulaGrammar.ValueExpressionStatement.Parse(coverlang);
            expression.Operands.Should().BeEquivalentTo(new Operand(){Name = "2020", Type = CoverLangDataType.Int});
            expression.Lines.Should().BeEquivalentTo("2020");
        }
        
        [Fact]
        public void Given_constant_negative_int_literal_When_Parse_Then_it_is_success()
        {
            var coverlang = @"-2020";

            var expression = FormulaGrammar.ValueExpressionStatement.Parse(coverlang);
            expression.Operands.Should().BeEquivalentTo(new Operand(){Name = "-2020", Type = CoverLangDataType.Int});
            expression.Lines.Should().BeEquivalentTo("-2020");
        }
        
        [Fact]
        public void Given_constant_string_literal_When_Parse_Then_it_is_success()
        {
            var coverlang = @"'abc'";

            var expression = FormulaGrammar.ValueExpressionStatement.Parse(coverlang);
            expression.Operands.Should().BeEquivalentTo(new Operand(){Name = "abc", Type = CoverLangDataType.String});
            expression.Lines.Should().BeEquivalentTo("abc");
        }
        
        [Fact]
        public void Given_constant_true_literal_When_Parse_Then_it_is_success()
        {
            var coverlang = @"true";

            var expression = FormulaGrammar.ValueExpressionStatement.Parse(coverlang);
            expression.Operands.Should().BeEquivalentTo(new Operand(){Name = "true", Type = CoverLangDataType.Bool});
            expression.Lines.Should().BeEquivalentTo("true");

        }
        
        [Fact]
        public void Given_constant_false_literal_When_Parse_Then_it_is_success()
        {
            var coverlang = @"false";

            var expression = FormulaGrammar.ValueExpressionStatement.Parse(coverlang);
            expression.Operands.Should().BeEquivalentTo(new Operand(){Name = "false", Type = CoverLangDataType.Bool});
            expression.Lines.Should().BeEquivalentTo("false");

        }
        
    }
      
      
    public class ValueExpressionConstantsOperationsCaptureTests
    {
        private readonly ITestOutputHelper _output;

        public ValueExpressionConstantsOperationsCaptureTests(ITestOutputHelper output)
        {
            _output = output;
        }
        
        
        [Fact]
        public void Given_constant_int_literal_When_Parse_Then_it_is_success()
        {
            var coverlang = @"2020+10";

            var expression = FormulaGrammar.ValueExpressionStatement.Parse(coverlang);
            expression.Operands.Should().BeEquivalentTo(new Operand(){Name = "2020", Type = CoverLangDataType.Int},
                new Operand()
                {
                    Name="10",
                    Type = CoverLangDataType.Int
                });
            expression.Lines.Should().BeEquivalentTo("2020+10");
        }
        
        [Fact]
        public void Given_constant_negative_int_literal_When_Parse_Then_it_is_success()
        {
            var coverlang = @"1-(2020+10)";

            var expression = FormulaGrammar.ValueExpressionStatement.Parse(coverlang);
            expression.Operands.Should().BeEquivalentTo(new Operand(){Name = "2020", Type = CoverLangDataType.Int},
                new Operand()
                {
                    Name="10",
                    Type = CoverLangDataType.Int
                });
        }
        
        [Fact]
        public void Given_constant_string_literal_When_Parse_Then_it_is_success()
        {
            var coverlang = @"'abc'";

            var expression = FormulaGrammar.ValueExpressionStatement.Parse(coverlang);
            expression.Operands.Should().BeEquivalentTo(new Operand(){Name = "abc", Type = CoverLangDataType.String});
        }
        
        [Fact]
        public void Given_constant_true_literal_When_Parse_Then_it_is_success()
        {
            var coverlang = @"true";

            var expression = FormulaGrammar.ValueExpressionStatement.Parse(coverlang);
            expression.Operands.Should().BeEquivalentTo(new Operand(){Name = "true", Type = CoverLangDataType.Bool});
        }
        
        [Fact]
        public void Given_constant_false_literal_When_Parse_Then_it_is_success()
        {
            var coverlang = @"false";

            var expression = FormulaGrammar.ValueExpressionStatement.Parse(coverlang);
            expression.Operands.Should().BeEquivalentTo(new Operand(){Name = "false", Type = CoverLangDataType.Bool});
        }
        
    }
}