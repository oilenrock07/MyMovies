namespace MyMovies.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedRelatedMovie : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Movies", "Year", c => c.String());
            AddColumn("dbo.Movies", "RelatedMovie", c => c.String());
            AddColumn("dbo.MovieXPaths", "Year", c => c.String());
            AddColumn("dbo.MovieXPaths", "RelatedMovie", c => c.String());
            DropColumn("dbo.Movies", "Duration");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Movies", "Duration", c => c.String());
            DropColumn("dbo.MovieXPaths", "RelatedMovie");
            DropColumn("dbo.MovieXPaths", "Year");
            DropColumn("dbo.Movies", "RelatedMovie");
            DropColumn("dbo.Movies", "Year");
        }
    }
}
