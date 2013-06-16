namespace SchoolReports.Models
{
    using System.Collections.Generic;
    using System.ComponentModel;

    public class TestResultGroup
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<TestResult> TestResults { get; set; }        
    }
}
