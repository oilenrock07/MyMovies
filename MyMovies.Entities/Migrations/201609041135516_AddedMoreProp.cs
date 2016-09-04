namespace MyMovies.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedMoreProp : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Movies", "Country", c => c.String());
            AddColumn("dbo.Movies", "Language", c => c.String());
            AddColumn("dbo.Movies", "AlsoKnownAs", c => c.String());
            AddColumn("dbo.Movies", "Budget", c => c.String());
            AddColumn("dbo.Movies", "Gross", c => c.String());
            AddColumn("dbo.MovieXPaths", "Country", c => c.String());
            AddColumn("dbo.MovieXPaths", "Language", c => c.String());
            AddColumn("dbo.MovieXPaths", "AlsoKnownAs", c => c.String());
            AddColumn("dbo.MovieXPaths", "Budget", c => c.String());
            AddColumn("dbo.MovieXPaths", "Gross", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.MovieXPaths", "Gross");
            DropColumn("dbo.MovieXPaths", "Budget");
            DropColumn("dbo.MovieXPaths", "AlsoKnownAs");
            DropColumn("dbo.MovieXPaths", "Language");
            DropColumn("dbo.MovieXPaths", "Country");
            DropColumn("dbo.Movies", "Gross");
            DropColumn("dbo.Movies", "Budget");
            DropColumn("dbo.Movies", "AlsoKnownAs");
            DropColumn("dbo.Movies", "Language");
            DropColumn("dbo.Movies", "Country");
        }
    }
}
