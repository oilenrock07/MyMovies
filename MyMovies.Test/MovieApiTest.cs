using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;
using MyMovies.Entities;
using MyMovies.Infrastructure.Implementations;
using MyMovies.Repository.Implementations;
using MyMovies.Web.BusinessLogic;
using MyMovies.Repository.Interfaces;
using MyMovies.Infrastructure.Interfaces;

namespace MyMovies.Test
{
    [TestClass]
    public class MovieApiTest
    {
        private readonly IMovieXPathRepository _xPathRepository;

        public MovieApiTest()
        {
            IDatabaseFactory databaseFactory = new DatabaseFactory(new MovieContext());
            _xPathRepository = new MovieXPathRepository(databaseFactory, null);
        }

        [TestMethod]
        public void ImdbNormalLayout()
        {
            //Spiderman: tt0468569
            //Dial M For Murder (old movie): tt0046912
            var scrapper = new ImdbScrapper(_xPathRepository);
            var movie = scrapper.GetMovie("tt0046912");
        }

        [TestMethod]
        public void ImdbWithPosterTest()
        {
            //Spiderman: tt0145487
            //Inception: tt1375666

            var scrapper = new ImdbScrapper(_xPathRepository);
            var movie = scrapper.GetMovie("tt0046912");
        }

        [TestMethod]
        public void ImdbWithSeriesTest()
        {
            //Walking Dead: tt1520211

            var scrapper = new ImdbScrapper(_xPathRepository);
            var movie = scrapper.GetMovie("tt1520211");
        }

        
    }
}
