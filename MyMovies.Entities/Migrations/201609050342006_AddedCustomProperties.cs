namespace MyMovies.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedCustomProperties : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Movies", "DateCreated", c => c.DateTime(nullable: false));
            AddColumn("dbo.Movies", "Location", c => c.String());
            AddColumn("dbo.Movies", "Remarks", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Movies", "Remarks");
            DropColumn("dbo.Movies", "Location");
            DropColumn("dbo.Movies", "DateCreated");
        }
    }
}
