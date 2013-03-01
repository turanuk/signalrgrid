namespace SignalRGridDemo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLockedProperty : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Employees", "Locked", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Employees", "Locked");
        }
    }
}
