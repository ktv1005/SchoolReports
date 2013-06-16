namespace SchoolReports.Models
{
    using SchoolReports.Migrations;
    using System.Data.Entity;

    public class ReportContext : DbContext
    {
        public DbSet<Period> Periods { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Class> Classes { get; set; }

        public DbSet<StudentInClass> StudentInClasses { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<TestGroup> TestGroups { get; set; }
        public DbSet<Test> Tests { get; set; }
        public DbSet<Question> Questions { get; set; }

        public DbSet<ReportResult> ReportResults { get; set; }
        public DbSet<TestResultGroup> TestResultGroups { get; set; }
        public DbSet<TestResult> TestResults { get; set; }
        public DbSet<Answer> Answers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ReportContext, Configuration>());
        }
    }
}
