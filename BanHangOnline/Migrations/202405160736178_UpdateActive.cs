namespace BanHangOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateActive : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tb_Category", "isActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.tb_News", "isActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.tb_Posts", "isActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.tb_Product", "isActive", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tb_Product", "isActive");
            DropColumn("dbo.tb_Posts", "isActive");
            DropColumn("dbo.tb_News", "isActive");
            DropColumn("dbo.tb_Category", "isActive");
        }
    }
}
