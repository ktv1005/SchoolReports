namespace SchoolReports.Models
{
    using System.Collections.Generic;
    using System.Linq;

    public class Report
    {
        public int Id { get; set; }

        public ReportTypes ReportType { get; set; }
        public int Level { get; set; }
        public int PeriodId { get; set; }

        public virtual Period Period { get; set; }

        public virtual ICollection<TestGroup> TestGroups { get; set; }

        public ReportResult CreateReportResult(int periodId, int studentInClassId, ReportContext reportContext)
        {
            ReportResult reportResult = reportContext.ReportResults.Create();

            reportResult.PeriodId = periodId;
            reportResult.StudentInClassId = studentInClassId;
            reportResult.ReportType = this.ReportType;
            reportResult.TestResultGroups = this.TestGroups.Select(testGroup => testGroup.CreateTestResults(reportContext)).ToList();

            return reportResult;
        }

        public static Report CreateReport(int level, ReportTypes reportType, int currentPeriodId, ReportContext reportContext)
        {
            Report report = reportContext.Reports.Create();

            report.Level = level;
            report.ReportType = reportType;
            report.PeriodId = currentPeriodId;
            report.Period = reportContext.Periods.Find(currentPeriodId);
            report.TestGroups = TestGroup.CreateTestGroups(reportType, reportContext);

            return report;
        }
    }
}