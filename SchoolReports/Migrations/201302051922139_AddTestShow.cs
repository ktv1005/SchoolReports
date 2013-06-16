namespace SchoolReports.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTestShow : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tests", "Show", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tests", "Show");
        }
    }
}
