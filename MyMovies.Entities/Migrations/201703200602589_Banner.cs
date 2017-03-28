namespace MyMovies.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Banner : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Banners",
                c => new
                    {
                        BannerId = c.Int(nullable: false, identity: true),
                        Poster = c.String(),
                        TextColor = c.String(),
                        Identifier = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        Type = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.BannerId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Banners");
        }
    }
}
