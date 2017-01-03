namespace MyMovies.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RelatedMovies : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MovieXPaths", "RelatedRoot", c => c.String());
            AddColumn("dbo.MovieXPaths", "RelatedDirectors", c => c.String());
            AddColumn("dbo.MovieXPaths", "RelatedStars", c => c.String());
            AddColumn("dbo.MovieXPaths", "RelatedRate", c => c.String());
            AddColumn("dbo.MovieXPaths", "RelatedTitle", c => c.String());
            AddColumn("dbo.MovieXPaths", "RelatedYear", c => c.String());
            AddColumn("dbo.MovieXPaths", "RelatedPoster", c => c.String());
            AddColumn("dbo.MovieXPaths", "RelatedGenre", c => c.String());
            AddColumn("dbo.MovieXPaths", "RelatedSummary", c => c.String());
            DropColumn("dbo.MovieXPaths", "RelatedMovie");
        }
        
        public override void Down()
        {
            
            AddColumn("dbo.MovieXPaths", "RelatedMovie", c => c.String());
            DropColumn("dbo.MovieXPaths", "RelatedSummary");
            DropColumn("dbo.MovieXPaths", "RelatedGenre");
            DropColumn("dbo.MovieXPaths", "RelatedPoster");
            DropColumn("dbo.MovieXPaths", "RelatedYear");
            DropColumn("dbo.MovieXPaths", "RelatedTitle");
            DropColumn("dbo.MovieXPaths", "RelatedRate");
            DropColumn("dbo.MovieXPaths", "RelatedStars");
            DropColumn("dbo.MovieXPaths", "RelatedDirectors");
            DropColumn("dbo.MovieXPaths", "RelatedRoot");
        }
    }
}
