namespace SchoolReports.Models
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;

    public class Test
    {
        public int Id { get; set; }
        
        [DisplayName("Onderwerp")]
        public string Subject { get; set; }

        [DisplayName("Thema")]
        public string Theme { get; set; }

        [DisplayName("Afwezig vraag tonen")]
        public bool ShowWasAbsent { get; set; }
        
        [DisplayName("Test weergeven")]
        public bool Show { get; set; }

        public virtual ICollection<Question> Questions { get; set; }

        public TestResult CreateTestResult(ReportContext reportContext)
        {
            TestResult testResult = reportContext.TestResults.Create();

            testResult.Subject = this.Subject;
            testResult.Theme = this.Theme;
            testResult.ShowWasAbsent = this.ShowWasAbsent;
            testResult.Answers = this.Questions.Where(question => !string.IsNullOrEmpty(question.Text))
                .Select(question => question.CreateAnswers(reportContext)).ToList();

            return testResult;
        }
        
        public static List<Test> CreateTestsForSwim(ReportContext reportContext)
        {
            Test test = reportContext.Tests.Create();

            test.ShowWasAbsent = true;
            test.Subject = string.Empty;
            test.Theme = string.Empty;
            test.Show = true;
            test.Questions = Question.CreateQuestionsForSwim(reportContext);

            return new List<Test> { test };
        }

        public static List<Test> CreateTestsForGymSkills(ReportContext reportContext)
        {
            Test testBall = reportContext.Tests.Create();

            testBall.ShowWasAbsent = true;
            testBall.Subject = "Balvaardigheid";
            testBall.Theme = string.Empty;
            testBall.Questions = Question.CreateQuestionsForGymSkill(reportContext);

            Test testBew = reportContext.Tests.Create();

            testBew.ShowWasAbsent = true;
            testBew.Subject = "Bewegingskunsten (turnvaardigheden)";
            testBew.Theme = string.Empty;
            testBew.Questions = Question.CreateQuestionsForGymSkill(reportContext);

            Test testCond = reportContext.Tests.Create();

            testCond.ShowWasAbsent = true;
            testCond.Subject = "Conditionele oefeningen";
            testCond.Theme = string.Empty;
            testCond.Questions = Question.CreateQuestionsForGymSkill(reportContext);

            Test testPrak = reportContext.Tests.Create();

            testPrak.ShowWasAbsent = true;
            testPrak.Subject = "Praktische vaardigheden";
            testPrak.Theme = string.Empty;
            testPrak.Questions = Question.CreateQuestionsForGymSkill(reportContext);

            Test testRit = reportContext.Tests.Create();

            testRit.ShowWasAbsent = true;
            testRit.Subject = "Ritme en expressie";
            testRit.Theme = string.Empty;
            testRit.Questions = Question.CreateQuestionsForGymSkill(reportContext);

            Test testSpr = reportContext.Tests.Create();

            testSpr.ShowWasAbsent = true;
            testSpr.Subject = "Springvaardigheden";
            testSpr.Theme = string.Empty;
            testSpr.Questions = Question.CreateQuestionsForGymSkill(reportContext);

            return new List<Test> { testBall, testBew, testCond, testPrak, testRit, testSpr };
        }

        public static List<Test> CreateTestsForGymAttitudes(ReportContext reportContext)
        {
            Test test = reportContext.Tests.Create();

            test.ShowWasAbsent = false;
            test.Subject = string.Empty;
            test.Theme = string.Empty;
            test.Show = true;
            test.Questions = Question.CreateQuestionsForGymAttitude(reportContext);

            return new List<Test> { test };
        }
    }
}
