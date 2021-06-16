namespace KindergartenWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCompanyMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Groups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GroupName = c.String(),
                        StaffId = c.Int(nullable: false),
                        CountOfChildren = c.Int(nullable: false),
                        YearOfCreation = c.Int(nullable: false),
                        GroupType_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GroupTypes", t => t.GroupType_Id)
                .ForeignKey("dbo.Staffs", t => t.StaffId, cascadeDelete: true)
                .Index(t => t.StaffId)
                .Index(t => t.GroupType_Id);
            
            CreateTable(
                "dbo.GroupTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NameOfType = c.String(),
                        Note = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Staffs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FullName = c.String(),
                        Adress = c.String(),
                        Phone = c.Int(nullable: false),
                        PositionId = c.Int(nullable: false),
                        Info = c.String(),
                        Reward = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Positions", t => t.PositionId, cascadeDelete: true)
                .Index(t => t.PositionId);
            
            CreateTable(
                "dbo.Positions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PositionName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Staffs", "PositionId", "dbo.Positions");
            DropForeignKey("dbo.Groups", "StaffId", "dbo.Staffs");
            DropForeignKey("dbo.Groups", "GroupType_Id", "dbo.GroupTypes");
            DropIndex("dbo.Staffs", new[] { "PositionId" });
            DropIndex("dbo.Groups", new[] { "GroupType_Id" });
            DropIndex("dbo.Groups", new[] { "StaffId" });
            DropTable("dbo.Positions");
            DropTable("dbo.Staffs");
            DropTable("dbo.GroupTypes");
            DropTable("dbo.Groups");
        }
    }
}
