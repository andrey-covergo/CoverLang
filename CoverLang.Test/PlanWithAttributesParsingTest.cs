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
        public void Given_plan_with_single_attribute_When_parse_Then_recognize_all_attribute_fields()
        {
            var coverLang = @"
Plan 'your opportunity 5' 
 Has required Attribute start_date with type date
";
            var parsed = CoverLangGrammar.Plan.Parse(coverLang);
            parsed.Name.Should().Be("your opportunity 5");
            var expectedAttribute = new Attribute
                {Name = "start_date", Type = AttributeDataType.Date, IsRequired = true};
            parsed.Attributes.Should().BeEquivalentTo(expectedAttribute);
        }


        [Fact]
        public void Given_plan_with_many_attributes_When_parse_Then_recognize_all_attribute_fields()
        {
            var coverLang = @"
Plan 'your opportunity 5' 
 Has required attribute start_date with type date




          Has optional attribute start with type int



";
            var parsed = CoverLangGrammar.Plan.Parse(coverLang);
            parsed.Name.Should().Be("your opportunity 5");
            var expectedAttributeA = new Attribute
                {Name = "start_date", Type = AttributeDataType.Date, IsRequired = true};
            var expectedAttributeB = new Attribute {Name = "start", Type = AttributeDataType.Int, IsRequired = false};

            parsed.Attributes.Should().HaveCount(2);
            parsed.Attributes.Should().BeEquivalentTo(expectedAttributeA, expectedAttributeB);
        }
        
        [Fact]
        public void Given_plan_with_bad_attributes_When_parse_Then_readable_error_raised()
        {
            var coverLang = @"
Plan 'your opportunity 5' 
 Hash required attribute start_date with type date


          Hasa optional attribute start with type int



";
            var ex = CoverLangGrammar.Plan.Invoking(p=>p.Parse(coverLang))
                          .Should().Throw<ParseException>().Subject.First();
            
            _output.WriteLine("Expected error raised:");
            _output.WriteLine(ex.ToString());
        }
    }
}