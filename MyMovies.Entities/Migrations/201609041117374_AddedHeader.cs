namespace MyMovies.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedHeader : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MovieXPaths", "Header", c => c.String());
            DropColumn("dbo.MovieXPaths", "Poster2");
            DropColumn("dbo.MovieXPaths", "Directors2");
            DropColumn("dbo.MovieXPaths", "Writers2");
            DropColumn("dbo.MovieXPaths", "Stars2");
        }
        
        public override void Down()
        {
            AddColumn("dbo.MovieXPaths", "Stars2", c => c.String());
            AddColumn("dbo.MovieXPaths", "Writers2", c => c.String());
            AddColumn("dbo.MovieXPaths", "Directors2", c => c.String());
            AddColumn("dbo.MovieXPaths", "Poster2", c => c.String());
            DropColumn("dbo.MovieXPaths", "Header");
        }
    }
}
