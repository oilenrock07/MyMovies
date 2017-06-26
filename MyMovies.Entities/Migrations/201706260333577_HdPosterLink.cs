namespace MyMovies.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HdPosterLink : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Movies", "HdPosterLink", c => c.String());
            DropColumn("dbo.Movies", "OriginalPosterUrl");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Movies", "OriginalPosterUrl", c => c.String());
            DropColumn("dbo.Movies", "HdPosterLink");
        }
    }
}
