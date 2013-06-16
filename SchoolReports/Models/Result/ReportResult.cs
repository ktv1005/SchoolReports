namespace SchoolReports.Models
{
    using System.Collections.Generic;

    public class ReportResult
    {
        public int Id { get; set; }

        public ReportTypes ReportType { get; set; }

        public int PeriodId { get; set; }
        public int StudentInClassId { get; set; }

        public virtual ICollection<TestResultGroup> TestResultGroups { get; set; }
    }
}