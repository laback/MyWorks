namespace KindergartenWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class test : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Groups", new[] { "GroupType_Id" });
            AddColumn("dbo.Groups", "GroupType_Id1", c => c.Int());
            AlterColumn("dbo.Groups", "GroupType_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.Groups", "GroupType_Id1");
            AddForeignKey("dbo.Groups", "GroupType_Id1", "dbo.GroupTypes", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Groups", "GroupType_Id1", "dbo.GroupTypes");
            DropIndex("dbo.Groups", new[] { "GroupType_Id1" });
            AlterColumn("dbo.Groups", "GroupType_Id", c => c.Int());
            DropColumn("dbo.Groups", "GroupType_Id1");
            CreateIndex("dbo.Groups", "GroupType_Id");
            AddForeignKey("dbo.Groups", "GroupType_Id", "dbo.GroupTypes", "Id");
        }
    }
}
