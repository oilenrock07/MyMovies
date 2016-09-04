namespace MyMovies.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedMorePropertiesWhenMovieHasTrailer : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MovieXPaths", "Poster2", c => c.String());
            AddColumn("dbo.MovieXPaths", "Stars2", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.MovieXPaths", "Stars2");
            DropColumn("dbo.MovieXPaths", "Poster2");
        }
    }
}
