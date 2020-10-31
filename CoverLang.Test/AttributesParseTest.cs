using FluentAssertions;
using Sprache;
using Xunit;

namespace CoverLang.Test
{
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
    }
}