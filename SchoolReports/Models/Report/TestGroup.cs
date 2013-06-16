namespace SchoolReports.Models
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;

    public class TestGroup
    {
        public int Id { get; set; }

        [DisplayName("Groepnaam")]
        public string Name { get; set; }

        public virtual ICollection<Test> Tests { get; set; }

        public TestResultGroup CreateTestResults(ReportContext reportContext)
        {
            TestResultGroup testResultGroup = reportContext.TestResultGroups.Create();

            testResultGroup.Name = this.Name;
            testResultGroup.TestResults = this.Tests.Where(test => test.Show)
                .Select(test => test.CreateTestResult(reportContext)).ToList();

            return testResultGroup;
        }
        
        public static List<TestGroup> CreateTestGroups(ReportTypes reportType, ReportContext reportContext)
        {
            switch (reportType)
            {
                case ReportTypes.Swim:
                    return CreateTestGroupsForSwim(reportContext);
                case ReportTypes.Gym:
                    return CreateTestGroupsForGym(reportContext);
                default:
                    return new List<TestGroup>();
            }            
        }

        private static List<TestGroup> CreateTestGroupsForSwim(ReportContext reportContext)
        {
            TestGroup testGroup = reportContext.TestGroups.Create();

            testGroup.Name = string.Empty;
            testGroup.Tests = Test.CreateTestsForSwim(reportContext);

            return new List<TestGroup> { testGroup };
        }

        private static List<TestGroup> CreateTestGroupsForGym(ReportContext reportContext)
        {
            TestGroup testGroup1 = reportContext.TestGroups.Create();

            testGroup1.Name = "Bewegingsopvoeding vaardigheden";
            testGroup1.Tests = Test.CreateTestsForGymSkills(reportContext);                  

            TestGroup testGroup2 = reportContext.TestGroups.Create();

            testGroup2.Name = "Bewegingsopvoeding attitudes";
            testGroup2.Tests = Test.CreateTestsForGymAttitudes(reportContext);

            return new List<TestGroup> { testGroup1, testGroup2 };
        }
    }
}