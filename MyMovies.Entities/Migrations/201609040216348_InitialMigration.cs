namespace MyMovies.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Movies",
                c => new
                    {
                        MovieId = c.Int(nullable: false, identity: true),
                        ImdbId = c.String(),
                        Title = c.String(),
                        Rate = c.Double(nullable: false),
                        Runtime = c.String(),
                        Rating = c.String(),
                        DateReleased = c.String(),
                        Poster = c.String(),
                        Directors = c.String(),
                        Writers = c.String(),
                        Stars = c.String(),
                        Summary = c.String(),
                        Genre = c.String(),
                    })
                .PrimaryKey(t => t.MovieId);
            
            CreateTable(
                "dbo.MovieXPaths",
                c => new
                    {
                        MovieXPathId = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Rate = c.String(),
                        Runtime = c.String(),
                        Rating = c.String(),
                        DateReleased = c.String(),
                        Poster = c.String(),
                        Directors = c.String(),
                        Writers = c.String(),
                        Stars = c.String(),
                        Summary = c.String(),
                        Genre = c.String(),
                    })
                .PrimaryKey(t => t.MovieXPathId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.MovieXPaths");
            DropTable("dbo.Movies");
        }
    }
}
