﻿namespace BanHangOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateProductCategory : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tb_ProductCategory", "Alias", c => c.String(nullable: false, maxLength: 150));
            AddColumn("dbo.tb_ProductCategory", "SeoTitle", c => c.String(maxLength: 500));
            AddColumn("dbo.tb_ProductCategory", "SeoDescription", c => c.String(maxLength: 250));
            AddColumn("dbo.tb_ProductCategory", "SeoKeyWords", c => c.String(maxLength: 150));
            AlterColumn("dbo.tb_ProductCategory", "Icon", c => c.String(maxLength: 250));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.tb_ProductCategory", "Icon", c => c.String());
            DropColumn("dbo.tb_ProductCategory", "SeoKeyWords");
            DropColumn("dbo.tb_ProductCategory", "SeoDescription");
            DropColumn("dbo.tb_ProductCategory", "SeoTitle");
            DropColumn("dbo.tb_ProductCategory", "Alias");
        }
    }
}
