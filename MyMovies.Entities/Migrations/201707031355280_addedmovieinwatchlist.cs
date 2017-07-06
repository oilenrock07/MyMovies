namespace MyMovies.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedmovieinwatchlist : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.WatchLists", "MovieId");
            AddForeignKey("dbo.WatchLists", "MovieId", "dbo.Movies", "MovieId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WatchLists", "MovieId", "dbo.Movies");
            DropIndex("dbo.WatchLists", new[] { "MovieId" });
        }
    }
}
