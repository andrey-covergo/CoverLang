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
        public void
            Given_optional_attribute_with_single_quoted_name_and_whitespaces_When_parse_Then_recognize_all_attribute_fields()
        {
            var coverLang = @"has optional Attribute 'start date' with      type    int";
            var parsed = CoverLangGrammar.Attribute.Parse(coverLang);
            parsed.Name.Should().Be("start date");
            parsed.Type.Should().Be(AttributeDataType.Int);
            parsed.IsRequired.Should().BeFalse();
        }

        [Fact]
        public void
            Given_optional_attribute_with_double_quoted_name_and_whitespaces_When_parse_Then_recognize_all_attribute_fields()
        {
            var coverLang = @"has optional    Attribute    ""start date"" with      type    bool";
            var parsed = CoverLangGrammar.Attribute.Parse(coverLang);
            parsed.Name.Should().Be("start date");
            parsed.Type.Should().Be(AttributeDataType.Bool);
            parsed.IsRequired.Should().BeFalse();
        }

        [Fact]
        public void
            Given_required_attribute_with_double_quoted_name_and_whitespaces_When_parse_Then_recognize_all_attribute_fields()
        {
            var coverLang = @"has   required    Attribute    ""start date"" with      type    string";
            var parsed = CoverLangGrammar.Attribute.Parse(coverLang);
            parsed.Name.Should().Be("start date");
            parsed.Type.Should().Be(AttributeDataType.String);
            parsed.IsRequired.Should().BeTrue();
        }
        
        [Fact]
        public void Given_bad_has_keyword_attribute_When_parse_Then_raise_an_error()
        {
            var coverLang = @"Hasa optional Attribute 'start date' with type date";
            
            var ex = CoverLangGrammar.Attribute.Invoking(p=>p.Parse(coverLang))
                .Should().Throw<ParseException>().Subject.First();
            
            _output.WriteLine("Expected error raised:");
            var message = ex.ToString();
            _output.WriteLine(message);

            message.Should().Contain(CoverLangGrammar.AttributeGrammar.HasKeywordSection);
        }
        
        [Fact]
        public void Given_bad_requires_keyword_attribute_When_parse_Then_raise_an_error()
        {
            var coverLang = @"Has optio_nal Attribute 'start date' with type date";
            
            var ex = CoverLangGrammar.Attribute.Invoking(p=>p.Parse(coverLang))
                .Should().Throw<ParseException>().Subject.First();
            
            _output.WriteLine("Expected error raised:");
            
            var message = ex.ToString();
            _output.WriteLine(ex.ToString());
            message.Should().Contain(CoverLangGrammar.AttributeGrammar.RequiredSection);
        }
        
        [Fact]
        public void Given_bad_attribute_keyword_attribute_When_parse_Then_raise_an_error()
        {
            var coverLang = @"Has optional tttribute 'start date' with type date";
            
            var ex = CoverLangGrammar.Attribute.Invoking(p=>p.Parse(coverLang))
                .Should().Throw<ParseException>().Subject.First();
            
            _output.WriteLine("Expected error raised:");
            var message = ex.ToString();
            _output.WriteLine(message);

            message.Should().Contain(CoverLangGrammar.AttributeGrammar.AttributeSection);
        }
        
        [Fact]
        public void Given_bad_data_type_keyword_attribute_When_parse_Then_raise_an_error()
        {
            var coverLang = @"Has optional attribute 'start date' withtype date";
            
            var ex = CoverLangGrammar.Attribute.Invoking(p=>p.Parse(coverLang))
                .Should().Throw<ParseException>().Subject.First();
            
            _output.WriteLine("Expected error raised:");
            var message = ex.ToString();
            _output.WriteLine(message);

            message.Should().Contain(CoverLangGrammar.AttributeGrammar.DataTypeSection);
        }
    }
}