namespace SchoolReports.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Periods",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        NumberSwim = c.Int(nullable: false),
                        NumberGym = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Students",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LastName = c.String(),
                        FirstName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.StudentInClasses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Year = c.Int(nullable: false),
                        StudentId = c.Int(nullable: false),
                        ClassId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Students", t => t.StudentId, cascadeDelete: true)
                .ForeignKey("dbo.Classes", t => t.ClassId, cascadeDelete: true)
                .Index(t => t.StudentId)
                .Index(t => t.ClassId);
            
            CreateTable(
                "dbo.Classes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Level = c.Int(nullable: false),
                        Location = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ReportResults",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ReportType = c.Int(nullable: false),
                        PeriodId = c.Int(nullable: false),
                        StudentInClassId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.StudentInClasses", t => t.StudentInClassId, cascadeDelete: true)
                .Index(t => t.StudentInClassId);
            
            CreateTable(
                "dbo.TestResultGroups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        ReportResult_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ReportResults", t => t.ReportResult_Id)
                .Index(t => t.ReportResult_Id);
            
            CreateTable(
                "dbo.TestResults",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Subject = c.String(),
                        Theme = c.String(),
                        ShowWasAbsent = c.Boolean(nullable: false),
                        WasAbsent = c.Boolean(nullable: false),
                        RemarkText = c.String(),
                        ResultText = c.String(),
                        ChosenSmiley = c.Int(nullable: false),
                        TestResultGroup_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TestResultGroups", t => t.TestResultGroup_Id)
                .Index(t => t.TestResultGroup_Id);
            
            CreateTable(
                "dbo.Answers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Result = c.Boolean(nullable: false),
                        QuestionText = c.String(),
                        RemarkText = c.String(),
                        TestResult_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TestResults", t => t.TestResult_Id)
                .Index(t => t.TestResult_Id);
            
            CreateTable(
                "dbo.Reports",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ReportType = c.Int(nullable: false),
                        Level = c.Int(nullable: false),
                        PeriodId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Periods", t => t.PeriodId, cascadeDelete: true)
                .Index(t => t.PeriodId);
            
            CreateTable(
                "dbo.TestGroups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Report_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Reports", t => t.Report_Id)
                .Index(t => t.Report_Id);
            
            CreateTable(
                "dbo.Tests",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Subject = c.String(),
                        Theme = c.String(),
                        ShowWasAbsent = c.Boolean(nullable: false),
                        TestGroup_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TestGroups", t => t.TestGroup_Id)
                .Index(t => t.TestGroup_Id);
            
            CreateTable(
                "dbo.Questions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Text = c.String(),
                        Test_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Tests", t => t.Test_Id)
                .Index(t => t.Test_Id);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Questions", new[] { "Test_Id" });
            DropIndex("dbo.Tests", new[] { "TestGroup_Id" });
            DropIndex("dbo.TestGroups", new[] { "Report_Id" });
            DropIndex("dbo.Reports", new[] { "PeriodId" });
            DropIndex("dbo.Answers", new[] { "TestResult_Id" });
            DropIndex("dbo.TestResults", new[] { "TestResultGroup_Id" });
            DropIndex("dbo.TestResultGroups", new[] { "ReportResult_Id" });
            DropIndex("dbo.ReportResults", new[] { "StudentInClassId" });
            DropIndex("dbo.StudentInClasses", new[] { "ClassId" });
            DropIndex("dbo.StudentInClasses", new[] { "StudentId" });
            DropForeignKey("dbo.Questions", "Test_Id", "dbo.Tests");
            DropForeignKey("dbo.Tests", "TestGroup_Id", "dbo.TestGroups");
            DropForeignKey("dbo.TestGroups", "Report_Id", "dbo.Reports");
            DropForeignKey("dbo.Reports", "PeriodId", "dbo.Periods");
            DropForeignKey("dbo.Answers", "TestResult_Id", "dbo.TestResults");
            DropForeignKey("dbo.TestResults", "TestResultGroup_Id", "dbo.TestResultGroups");
            DropForeignKey("dbo.TestResultGroups", "ReportResult_Id", "dbo.ReportResults");
            DropForeignKey("dbo.ReportResults", "StudentInClassId", "dbo.StudentInClasses");
            DropForeignKey("dbo.StudentInClasses", "ClassId", "dbo.Classes");
            DropForeignKey("dbo.StudentInClasses", "StudentId", "dbo.Students");
            DropTable("dbo.Questions");
            DropTable("dbo.Tests");
            DropTable("dbo.TestGroups");
            DropTable("dbo.Reports");
            DropTable("dbo.Answers");
            DropTable("dbo.TestResults");
            DropTable("dbo.TestResultGroups");
            DropTable("dbo.ReportResults");
            DropTable("dbo.Classes");
            DropTable("dbo.StudentInClasses");
            DropTable("dbo.Students");
            DropTable("dbo.Periods");
        }
    }
}
