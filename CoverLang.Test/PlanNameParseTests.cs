using System.Linq;
using FluentAssertions;
using Sprache;
using Xunit;
using Xunit.Abstractions;

namespace CoverLang.Test
{

    public class CoverGoLangExamples
    {
        
        public const string Bupa = @"
#this is a comment 
#this is a CoverLang representation of BUPA insurance example 
#https://www.figma.com/file/VP1mP2JDpdarkoJdbvXWAH/Bupa?node-id=248%3A127688

Plan 'Scheme 2'
    Has required Attribute 'start date' with type date 
    Has optional Attribute 'end date' with type date
    Has required Attribute 'sales comission rate' with type int 
    Has Attribute 'num of employees covered' with type int 
    Has Attribute AreAllEmployeesCovered with type bool
    Has Formula 'core pricing eligibility' returning bool as 
        if 'num of employees covered' > 10 or AreAllEmployeesCovered then true
	            
Pricing
if 'core pricing eligibility' is true then 
    formula 'Exclusive Options' returning int
        if 'sales comission rate' > 0 and 'sales comission rate' < 5 then 700
        if 'sales comission rate' >= 5 and 'sales comission rate' <10 then 740
        if 'sales comission rate' >= 10 and 'sales comission rate' <15 then 780
        if 'sales comission rate' >= 15 then 825
if 'core pricing eligibility' is not true then 1800

Benefits
    Rated limit once per year
    if 'benefit type' = 'Eye Exam' then
        Rated limit once per visit 
    if benefit = 'Comprehensive Eye exam'
        give benefit 'Comprehensive Eye exam'
        give benefit 'Full written report'
    if benefit = 'Retractive Eye exam'
        give benefit 'Retractive Eye exam'
";
    }
    public class PlanNameParseTests
    {
        private readonly ITestOutputHelper _output;

        public PlanNameParseTests(ITestOutputHelper output)
        {
            _output = output;
        }
  

        [Fact]
        public void Given_coverLang_empty_plan_with_single_quoted_name_When_parse_Then_produce_empty_result()
        {
            var coverLang = @"
Plan 'Scheme 2'
";
            var parsed = PlanGrammar.Plan.Parse(coverLang);
            parsed.Name.Should().Be("Scheme 2");
        }

        [Fact]
        public void Given_coverLang_empty_plan_with_double_quoted_name_When_parse_Then_plan_is_parced_with_name()
        {
            var coverLang = @"
Plan ""Scheme 2""
";
            var parsed = PlanGrammar.Plan.Parse(coverLang);
            parsed.Name.Should().Be("Scheme 2");
        }


        [Fact]
        public void Given_coverLang_empty_plan_with_mono_name_When_parse_Then_produce_empty_result()
        {
            var coverLang = @"
Plan Scheme2
";
            var parsed = PlanGrammar.Plan.Parse(coverLang);
            parsed.Name.Should().Be("Scheme2");
        }

        [Fact]
        public void Given_coverLang_empty_plan_When_parse_identifier_Then_find_Plan_token()
        {
            var coverLang = @"Plan Scheme2";
            var plan = CoverLangGrammar.MonoIdentifier.Parse(coverLang);
            plan.Should().Be("Plan");
        }

        [Fact]
        public void Given_coverLang_invalid_plan_When_parse_identifier_Then_exception()
        {
            var coverLang = @"
PlanT 'Scheme 2'
";
          var ex = FluentActions.Invoking(() => PlanGrammar.Plan.Parse(coverLang))
                .Should().Throw<ParseException>().Subject.First();
          
          _output.WriteLine("Expected error raised:");
          _output.WriteLine(ex.ToString());
        }
    }
}