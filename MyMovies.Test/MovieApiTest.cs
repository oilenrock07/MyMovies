using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;
using HtmlAgilityPack;
using MyMovies.Common.BusinessLogic;
using MyMovies.Entities;
using MyMovies.Infrastructure.Implementations;
using MyMovies.Repository.Implementations;
using MyMovies.Repository.Interfaces;
using MyMovies.Infrastructure.Interfaces;
using System.IO;
using System.Linq;
using MyMovies.Common.Extension;
using MyMovies.Web.ViewModels;
using System.Linq;

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
        public void LoadMovieFromFile()
        {
            var scrapper = new ImdbScrapper(_xPathRepository);
            var movie = scrapper.LoadMovieFromFile(@"C:\nightmare.txt");
        }

        [TestMethod]
        public void ImdbNormalLayout()
        {
            //Spiderman: tt0468569
            //Dial M For Murder (old movie): tt0046912
            var scrapper = new ImdbScrapper(_xPathRepository);
            var movie = scrapper.GetMovie("tt0137523");
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
            //Game Of Thrones: tt0944947
            var scrapper = new ImdbScrapper(_xPathRepository);
            var movie = scrapper.GetMovie("tt0944947");
        }

        [TestMethod]
        public void FilipinoMoviesTest()
        {
            //Ang tanging ina: tt0368323
            var scrapper = new ImdbScrapper(_xPathRepository);
            var movie = scrapper.GetMovie("tt0368323");
        }

        [TestMethod]
        public void GetRelatedMovies()
        {
            var doc = new HtmlDocument();
            var html = File.ReadAllText(@"C:\nightmare.txt");
            doc.LoadHtml(html);

            var relatedMoviesList = new List<Movie>();
            //var relatedMovies = doc.DocumentNode.SelectNodes("//*[contains(@class, \"rec_item\")]");
            var overviews = doc.DocumentNode.SelectNodes("//*[@class=\"rec_overview\"]");

            //foreach (var relatedMovie in relatedMovies)
            //{
            //    var link = relatedMovie.SelectSingleNode("a");
            //    var image = link.SelectSingleNode("img");

            //    relatedMoviesList.Add(new Movie
            //    {
            //        Title = image.Attribute("title"), 
            //        Poster = image.Attribute("src"),
            //        ImdbId = relatedMovie.Attribute("data-tconst")
            //    });
            //}

            foreach (var overview in overviews)
            {
                //var image = overview.SelectSingleNode("//*[@class=\"loadlate rec_poster_img\"]");
                var genres = overview.SelectSingleNode(".//div[contains(@class, \"rec-cert-genre\")]");
                relatedMoviesList.Add(new Movie
                {
                    //Title = image.Attribute("title"),
                    //Poster = image.Attribute("src"),
                    ImdbId = overview.Attribute("data-tconst"),
                    Title = overview.SelectSingleNode(".//div[@class=\"rec-title\"]/a/b").InnerTextClean(),
                    Year = overview.SelectSingleNode(".//div[@class=\"rec-title\"]/span").InnerTextClean().Replace("(", "").Replace(")", ""),
                });
               
            }

        }
    }
}
