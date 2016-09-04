namespace MyMovies.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class returnPoster2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MovieXPaths", "Poster2", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.MovieXPaths", "Poster2");
        }
    }
}
