namespace SchoolReports.Models
{
    using System.Collections.Generic;
    using System.Linq;

    public class StudentInClass
    {
        public int Id { get; set; }

        public int Year { get; set; }
        public int StudentId { get; set; }
        public int ClassId { get; set; }

        public SchoolYear SchoolYear
        {
            get
            {
                return new SchoolYear() { Year = this.Year };
            }
        }

        public virtual Student Student { get; set; }
        public virtual Class Class { get; set; }

        public string DisplayName
        {
            get
            {
                return this.Class.Name + " - " + this.Student.FullName;
            }
        }

        public virtual ICollection<ReportResult> ReportResults { get; set; }

        public ReportResult GetReportResultForPeriod(int periodId, ReportTypes reportType)
        {
            if (reportType == ReportTypes.Swim)
            {
                return this.GetReportResultForPeriod(this.SwimReportResults, periodId);
            }

            return this.GetReportResultForPeriod(this.GymReportResults, periodId);
        }

        public bool HasReportResultForPeriod(int periodId, ReportTypes reportType)
        {
            if (reportType == ReportTypes.Swim)
            {
                return this.HasReportResultForPeriod(this.SwimReportResults, periodId);
            }

            return this.HasReportResultForPeriod(this.GymReportResults, periodId);
        }

        public ReportResult CreateReportResultForPeriod(int periodId, ReportTypes reportType, ReportContext reportContext)
        {
            Report report = reportContext.Reports.Single(r => r.PeriodId == periodId &&
                                                              r.ReportType == reportType &&
                                                              r.Level == this.Class.Level);

            return report.CreateReportResult(periodId, this.Id, reportContext);
        }

        private IEnumerable<ReportResult> SwimReportResults
        {
            get
            {
                return this.ReportResults.Where(report => report.ReportType == ReportTypes.Swim);
            }
        }

        private IEnumerable<ReportResult> GymReportResults
        {
            get
            {
                return this.ReportResults.Where(report => report.ReportType == ReportTypes.Gym);
            }
        }

        private bool HasReportResultForPeriod(IEnumerable<ReportResult> reportResults, int periodId)
        {
            return reportResults.Count(reportResult => reportResult.PeriodId == periodId) > 0;
        }

        private ReportResult GetReportResultForPeriod(IEnumerable<ReportResult> reportResults, int periodId)
        {
            return reportResults.Single(reportResult => reportResult.PeriodId == periodId);
        }
    }
}
