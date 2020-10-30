using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using FluentAssertions;
using Sprache;
using Xunit;

namespace CoverLang.Test
{
    public class CoverLangNode
    {
        public string Name { get; }
        public string Content { get; }
    }
    public class Attribute:CoverLangNode
    {
    }

    public class Formula:CoverLangNode
    {
    }
    

    public class Plan
    {
        public string Name { get; set; }
        public List<Attribute> Attributes { get; }
        public List<Formula> Formulas { get; }
        public string Pricing { get; }
        public string Benefits { get; }
    }
    
    
    public class UnitTest1
    {
        private const string MaxLang = @"
Plan 'Scheme 2'
Has required Attribute 'start date' with type date 
    Has optional Attribute 'end date' with type date
    Has required Attribute 'sales comission rate' with type int 
    Has Attribute 'num of employees covered' with type int 
    Has Attribute AreAllEmployeesCovered with type bool
    Has Formula 'core pricing eligibility' returning bool as 
if 'num of employees covered' > 10 or AreAllEmployeesCovered  then true
	            
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
        
        [Fact]
        public void Given_coverLang_empty_plan_with_single_quoted_name_When_parse_Then_produce_empty_result()
        {

            var coverLang = @"
Plan 'Scheme 2'
";
            var parsed = CoverLangGrammar.Plan.Parse(coverLang);
            parsed.Name.Should().Be("Scheme 2");
        }   
        
        [Fact]
        public void Given_coverLang_empty_plan_with_mono_name_When_parse_Then_produce_empty_result()
        {

            var coverLang = @"
Plan Scheme2
";
            var parsed = CoverLangGrammar.Plan.Parse(coverLang);
            parsed.Name.Should().Be("Scheme2");
        }   

        
        
        [Fact]
        public void Given_coverLang_empty_plan_When_parse_identifier_Then_find_Plan_token()
        {

            var coverLang = @"
Plan 'Scheme 2'
";
            var plan = CoverLangGrammar.MonoIdentifier.Parse(coverLang);
            plan.Should().Be("Plan");
        } 
        
        [Fact]
        public void Given_coverLang_empty_plan_When_parse_plan_identifier_Then_find_Plan_token()
        {

            var coverLang = @"
Plan 'Scheme 2'
";
            var plan = CoverLangGrammar.PlanIdentifier.Parse(coverLang);
            plan.Should().Be("Plan");
        } 
        
        [Fact]
        public void Given_coverLang_invalid_plan_When_parse_identifier_Then_exception()
        {

            var coverLang = @"
PlanT 'Scheme 2'
";
            FluentActions.Invoking(() => CoverLangGrammar.PlanIdentifier.Parse(coverLang))
                .Should().Throw<ParseException>();
        }
    }


    public class CoverLangGrammar
    {
        public static readonly Parser<string> MonoIdentifier = Parse.Letter.AtLeastOnce().Text().Token();

        public static readonly Parser<string> PlanIdentifier =
            from text in Parse.Letter.AtLeastOnce().Text().Token() where text == "Plan" select text;
       

        public static readonly Parser<string> QuotedText =
            (from open in Parse.Char('"')
                from content in Parse.CharExcept('"').Many().Text()
                from close in Parse.Char('"')
                select content).Token();
        
        public static readonly Parser<string> SingleQuotedText =
            (from open in Parse.Char('\'')
                from content in Parse.CharExcept('\'').Many().Text()
                from close in Parse.Char('\'')
                select content).Token();
        
        public static readonly Parser<string> Identifier =
            from token in MonoIdentifier.Or(SingleQuotedText) 
            select token;
        
        public static readonly Parser<Plan> Plan =
                        from planToken in PlanIdentifier
                        from name in Identifier
                        select new Plan{Name = name};
    }
    public class CoverLangParser
    {
        
        public Plan ParsePlan(string dsl)
        {
         
            
            throw new NotImplementedException();
        }
    }
}