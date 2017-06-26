namespace MyMovies.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PosterUrlAndMovieWatchList : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Movies", "OriginalPosterUrl", c => c.String());
            AlterColumn("dbo.WatchLists", "UserId", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.WatchLists", "UserId", c => c.Int(nullable: false));
            DropColumn("dbo.Movies", "OriginalPosterUrl");
        }
    }
}
