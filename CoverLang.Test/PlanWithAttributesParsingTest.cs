using System.Linq;
using FluentAssertions;
using Sprache;
using Xunit;
using Xunit.Abstractions;

namespace CoverLang.Test
{
    public class PlanWithAttributesParsingTest
    {
        private readonly ITestOutputHelper _output;

        public PlanWithAttributesParsingTest(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]

        public void PlayWithEOL()
        {
            var coverLang = @"
Plan 
    Has
";

            var parser = 
                         from leading in Parse.WhiteSpace.Many()   
                         from text in Parse.LetterOrDigit.Many().Text()
                         from trailing in Parse.WhiteSpace.Until(Parse.LineEnd)
                        // from eol in Parse.LineEnd
                         from indent in Parse.WhiteSpace.Repeat(4)
                         from text2 in Parse.LetterOrDigit.Many().Text()
                         select text + text2;
            _output.WriteLine(parser.Parse(coverLang));
        }
        [Fact]
        public void Given_plan_with_single_attribute_When_parse_Then_recognize_all_attribute_fields()
        {
            var coverLang = @"
Plan 'your opportunity 5' 
    required attribute start_date with type date 
";
            var parsed = PlanGrammar.Plan.Parse(coverLang);
            parsed.Name.Should().Be("your opportunity 5");
            var expectedAttribute = new Attribute
                {Name = "start_date", Type = CoverLangDataType.Date, IsRequired = true};
            parsed.Attributes.Should().BeEquivalentTo(expectedAttribute);
        }


        [Fact]
        public void Given_plan_with_many_attributes_When_parse_Then_recognize_all_attribute_fields()
        {
            var coverLang = @"
Plan 'your opportunity 5'
    required attribute start_date with type date
    optional attribute start with type int 
";

        var parsed = PlanGrammar.Plan.Parse(coverLang);
            parsed.Name.Should().Be("your opportunity 5");
            var expectedAttributeA = new Attribute
                {Name = "start_date", Type = CoverLangDataType.Date, IsRequired = true};
            var expectedAttributeB = new Attribute {Name = "start", Type = CoverLangDataType.Int, IsRequired = false};

            parsed.Attributes.Should().HaveCount(2);
            parsed.Attributes.Should().BeEquivalentTo(expectedAttributeA, expectedAttributeB);
        }
        
        
//         [Fact]
//         public void Given_plan_with_bad_attributes_When_parse_Then_readable_error_raised()
//         {
//             var coverLang = @"
// Plan 'your opportunity 5' 
// {
//     Has requiredU attribute start_date with type date
//     Has_a optional attribute start with type int
// }";
//
//             var a = PlanGrammar.Plan.Parse(coverLang);
//             PlanGrammar.Plan.Invoking(p=>p.Parse(coverLang))
//                           .Should().Throw<ParseException>()
//                           .WithMessageContaining(AttributeGrammar.Parts.RequiredKeyword);
//         }
    }
}