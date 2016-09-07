namespace MyMovies.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addmoreproperties : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Reviews",
                c => new
                    {
                        ReviewId = c.Int(nullable: false, identity: true),
                        MovieId = c.Int(nullable: false),
                        Reviews = c.String(),
                    })
                .PrimaryKey(t => t.ReviewId);
            
            AddColumn("dbo.Movies", "FileName", c => c.String(nullable: false));
            AddColumn("dbo.Movies", "FileSize", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Movies", "FileSize");
            DropColumn("dbo.Movies", "FileName");
            DropTable("dbo.Reviews");
        }
    }
}
