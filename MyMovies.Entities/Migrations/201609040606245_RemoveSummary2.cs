namespace MyMovies.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveSummary2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MovieXPaths", "Directors2", c => c.String());
            DropColumn("dbo.MovieXPaths", "Summary2");
        }
        
        public override void Down()
        {
            AddColumn("dbo.MovieXPaths", "Summary2", c => c.String());
            DropColumn("dbo.MovieXPaths", "Directors2");
        }
    }
}
