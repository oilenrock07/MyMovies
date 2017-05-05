namespace MyMovies.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FavoritesAndPlaylist : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MoviePlayLists",
                c => new
                    {
                        MoviePlayListId = c.Int(nullable: false, identity: true),
                        PlayListId = c.Int(nullable: false),
                        MovieId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.MoviePlayListId);
            
            CreateTable(
                "dbo.PlayLists",
                c => new
                    {
                        PlayListId = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        PlayListName = c.String(maxLength: 250),
                        Description = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.PlayListId);
            
            CreateTable(
                "dbo.WatchLists",
                c => new
                    {
                        WatchListId = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        MovieId = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        DateAdded = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.WatchListId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.WatchLists");
            DropTable("dbo.PlayLists");
            DropTable("dbo.MoviePlayLists");
        }
    }
}
