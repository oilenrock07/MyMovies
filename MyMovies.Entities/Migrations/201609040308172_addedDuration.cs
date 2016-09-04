namespace MyMovies.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedDuration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Movies", "Duration", c => c.String());
            AddColumn("dbo.MovieXPaths", "Duration", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.MovieXPaths", "Duration");
            DropColumn("dbo.Movies", "Duration");
        }
    }
}
