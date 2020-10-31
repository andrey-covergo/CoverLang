using System.Linq;
using FluentAssertions;
using Sprache;
using Xunit;
using Xunit.Abstractions;

namespace CoverLang.Test
{
    public class AttributesParseTest
    {
        private readonly ITestOutputHelper _output;

        public AttributesParseTest(ITestOutputHelper output)
        {
            _output = output;
        }
        [Fact]
        public void Given_required_attribute_with_mono_name_When_parse_Then_recognize_all_attribute_fields()
        {
            var coverLang = @"required attribute start_date with type date";
            var parsed = AttributeGrammar.Attribute.Parse(coverLang);
            parsed.Name.Should().Be("start_date");
            parsed.Type.Should().Be(CoverLangDataType.Date);
            parsed.IsRequired.Should().BeTrue();
        }

        [Fact]
        public void Given_optional_attribute_with_single_quoted_name_When_parse_Then_recognize_all_attribute_fields()
        {
            var coverLang = @"optional attribute 'start date' with type date";
            var parsed = AttributeGrammar.Attribute.Parse(coverLang);
            parsed.Name.Should().Be("start date");
            parsed.Type.Should().Be(CoverLangDataType.Date);
            parsed.IsRequired.Should().BeFalse();
        }

        [Fact]
        public void
            Given_optional_attribute_with_single_quoted_name_and_whitespaces_When_parse_Then_recognize_all_attribute_fields()
        {
            var coverLang = @"optional attribute 'start date' with      type    int";
            var parsed = AttributeGrammar.Attribute.Parse(coverLang);
            parsed.Name.Should().Be("start date");
            parsed.Type.Should().Be(CoverLangDataType.Int);
            parsed.IsRequired.Should().BeFalse();
        }

        [Fact]
        public void
            Given_optional_attribute_with_double_quoted_name_and_whitespaces_When_parse_Then_recognize_all_attribute_fields()
        {
            var coverLang = @"optional    attribute    ""start date"" with      type    bool";
            var parsed = AttributeGrammar.Attribute.Parse(coverLang);
            parsed.Name.Should().Be("start date");
            parsed.Type.Should().Be(CoverLangDataType.Bool);
            parsed.IsRequired.Should().BeFalse();
        }

        [Fact]
        public void
            Given_required_attribute_with_double_quoted_name_and_whitespaces_When_parse_Then_recognize_all_attribute_fields()
        {
            var coverLang = @"required    attribute    ""start date"" with      type    string";
            var parsed = AttributeGrammar.Attribute.Parse(coverLang);
            parsed.Name.Should().Be("start date");
            parsed.Type.Should().Be(CoverLangDataType.String);
            parsed.IsRequired.Should().BeTrue();
        }
        
        
        [Fact]
        public void Given_bad_requires_keyword_attribute_When_parse_Then_raise_an_error()
        {
            var coverLang = @"optio_nal attribute 'start date' with type date";

            var ex = AttributeGrammar.Attribute.Invoking(p => p.Parse(coverLang))
                .Should().Throw<ParseException>();
        }
        
        [Fact]
        public void Given_bad_attribute_keyword_attribute_When_parse_Then_raise_an_error()
        {
            var coverLang = @"optional tttribute 'start date' with type date";
            
            var ex = AttributeGrammar.Attribute.Invoking(p=>p.Parse(coverLang))
                .Should().Throw<ParseException>().Subject.First();
            
            _output.WriteLine("Expected error raised:");
            var message = ex.ToString();
            _output.WriteLine(message);

            message.Should().Contain(AttributeGrammar.Parts.AttributeKeyword);
        }
        
        [Fact]
        public void Given_bad_data_type_keyword_attribute_When_parse_Then_raise_an_error()
        {
            var coverLang = @"optional attribute 'start date' with_type date";
            
            AttributeGrammar.Attribute
                .Invoking(p=>p.Parse(coverLang))
                .Should().Throw<ParseException>()
                         .WithMessageContaining(AttributeGrammar.Parts.TypeKeyword);
        }
    }
}