using System.Linq;
using System.Net.Sockets;
using FluentAssertions;
using Sprache;
using Xunit;

namespace CoverLang.Test
{

    public class PlanWithAttributesParsingTest
    {
        [Fact]
        public void Given_plan_with_single_attribute_When_parse_Then_recognize_all_attribute_fields()
        {
        
            var coverLang = @"
Plan 'your opportunity 5' 
 Has required Attribute start_date with type date
";
            var parsed = CoverLangGrammar.Plan.Parse(coverLang);
            parsed.Name.Should().Be("your opportunity 5");
            var expectedAttribute = new Attribute(){Name="start_date", Type = AttributeDataType.Date, IsRequired = true};
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
                var expectedAttributeA = new Attribute(){Name="start_date", Type = AttributeDataType.Date, IsRequired = true};
                var expectedAttributeB = new Attribute(){Name="start", Type = AttributeDataType.Int, IsRequired = false};

                parsed.Attributes.Should().HaveCount(2);
                parsed.Attributes.Should().BeEquivalentTo(expectedAttributeA,expectedAttributeB);
            } 
        
        }
    

    public class AttributesParseTest
    {
             
        [Fact]
        public void Given_required_attribute_with_mono_name_When_parse_Then_recognize_all_attribute_fields()
        {
        
            var coverLang = @"Has required Attribute start_date with type date";
            var parsed = CoverLangGrammar.Attribute.Parse(coverLang);
            parsed.Name.Should().Be("start_date");
            parsed.Type.Should().Be(AttributeDataType.Date);
            parsed.IsRequired.Should().BeTrue();
        } 
        
        [Fact]
        public void Given_optional_attribute_with_single_quoted_name_When_parse_Then_recognize_all_attribute_fields()
        {
        
            var coverLang = @"Has optional Attribute 'start date' with type date";
            var parsed = CoverLangGrammar.Attribute.Parse(coverLang);
            parsed.Name.Should().Be("start date");
            parsed.Type.Should().Be(AttributeDataType.Date);
            parsed.IsRequired.Should().BeFalse();
        }   
        
        [Fact]
        public void Given_optional_attribute_with_single_quoted_name_and_whitespaces_When_parse_Then_recognize_all_attribute_fields()
        {
        
            var coverLang = @"has optional Attribute 'start date' with      type    int";
            var parsed = CoverLangGrammar.Attribute.Parse(coverLang);
            parsed.Name.Should().Be("start date");
            parsed.Type.Should().Be(AttributeDataType.Int);
            parsed.IsRequired.Should().BeFalse();
        }   
        
        [Fact]
        public void Given_optional_attribute_with_double_quoted_name_and_whitespaces_When_parse_Then_recognize_all_attribute_fields()
        {
        
            var coverLang = @"has optional    Attribute    ""start date"" with      type    bool";
            var parsed = CoverLangGrammar.Attribute.Parse(coverLang);
            parsed.Name.Should().Be("start date");
            parsed.Type.Should().Be(AttributeDataType.Bool);
            parsed.IsRequired.Should().BeFalse();
        }  

        [Fact]
        public void Given_required_attribute_with_double_quoted_name_and_whitespaces_When_parse_Then_recognize_all_attribute_fields()
        {
        
            var coverLang = @"has   required    Attribute    ""start date"" with      type    string";
            var parsed = CoverLangGrammar.Attribute.Parse(coverLang);
            parsed.Name.Should().Be("start date");
            parsed.Type.Should().Be(AttributeDataType.String);
            parsed.IsRequired.Should().BeTrue();
        }  

    }
}