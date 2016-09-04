namespace MyMovies.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedSummary2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MovieXPaths", "Summary2", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.MovieXPaths", "Summary2");
        }
    }
}
