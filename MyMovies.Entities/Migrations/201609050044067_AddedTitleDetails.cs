namespace MyMovies.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedTitleDetails : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MovieXPaths", "TitleDetails", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.MovieXPaths", "TitleDetails");
        }
    }
}
