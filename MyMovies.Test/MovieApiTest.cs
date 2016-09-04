using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;
using MyMovies.Entities;
using MyMovies.Infrastructure.Implementations;
using MyMovies.Repository.Implementations;
using MyMovies.Web.BusinessLogic;

namespace MyMovies.Test
{
    [TestClass]
    public class MovieApiTest
    {
        [TestMethod]
        public void ImdbScrapeTest()
        {
            var databaseFactory = new DatabaseFactory(new MovieContext());
            var xPathRepository = new MovieXPathRepository(databaseFactory, null);

            var scrapper = new ImdbScrapper(xPathRepository);
            var movie = scrapper.GetMovie("tt1375666");
        }
    }
}
