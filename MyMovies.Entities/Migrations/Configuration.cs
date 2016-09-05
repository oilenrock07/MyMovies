namespace MyMovies.Entities.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<MyMovies.Entities.MovieContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(MyMovies.Entities.MovieContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            context.MovieXPaths.AddOrUpdate(
              p => p.MovieXPathId,
              new MovieXPath
              {
                  Title= "//*[@id=\"title-overview-widget\"]/div[2]/div[2]/div/div[2]/div[2]/h1/text()",
                  Rate = "//*[@id=\"title-overview-widget\"]/div[2]/div[2]/div/div[1]/div[1]/div[1]/strong/span",
                  Rating = "//*[@id=\"title-overview-widget\"]/div[2]/div[2]/div/div[2]/div[2]/div/meta",
                  Duration = "//*[@id=\"title-overview-widget\"]/div[2]/div[2]/div/div[2]/div[2]/div/time",
                  Genre = "//div[@itemprop=\"genre\"]/a",
                  DateReleased = "//*[@id=\"title-overview-widget\"]/div[2]/div[2]/div/div[2]/div[2]/div/a[4]/text()",
                  Year = "//*[@id=\"titleYear\"]/a",
                  Summary = "//*[@id=\"titleStoryLine\"]/div[1]/p/text()",
                  Runtime = "//*[@id=\"title-overview-widget\"]/div[2]/div[2]/div/div[2]/div[2]/div/time",

                  //inside headers
                  Header = "//*[@id=\"title-overview-widget\"]",
                  Poster = "*//img[@itemprop=\"image\"]",
                  Stars = "//*[@itemprop=\"actors\"]",
                  Directors = "//*[@itemprop=\"director\"]",
                  Writers = "//*[@itemprop=\"creator\"]",

                  RelatedMovie = "//*[@id=\"title_recs\"]/div[1]/div/div[2]/div",
                  TitleDetails = "//*[@id=\"titleDetails\"]"
              }
            );
        }
    }
}
