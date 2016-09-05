namespace MyMovies.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedRequiredFieldToImdb : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Movies", "ImdbId", c => c.String(nullable: false));
            AlterColumn("dbo.Movies", "Title", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Movies", "Title", c => c.String());
            AlterColumn("dbo.Movies", "ImdbId", c => c.String());
        }
    }
}
