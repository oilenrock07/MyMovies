namespace MyMovies.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeposter2 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.MovieXPaths", "Poster2");
        }
        
        public override void Down()
        {
            AddColumn("dbo.MovieXPaths", "Poster2", c => c.String());
        }
    }
}
