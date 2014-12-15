namespace HobbyClue.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Avatar", c => c.String());
            AddColumn("dbo.AspNetUsers", "About", c => c.String());
            DropColumn("dbo.AspNetUsers", "HomeTown");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "HomeTown", c => c.String());
            DropColumn("dbo.AspNetUsers", "About");
            DropColumn("dbo.AspNetUsers", "Avatar");
        }
    }
}
