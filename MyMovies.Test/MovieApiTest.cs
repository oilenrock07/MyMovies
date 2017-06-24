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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
            var movie = scrapper.LoadMovieFromFile(@"C:\tt1038988.txt");
        }

        [TestMethod]
        public void ImdbNormalLayout()
        {
            //Spiderman: tt0468569
            //Dial M For Murder (old movie): tt0046912
            var scrapper = new ImdbScrapper(_xPathRepository);
            var movie = scrapper.GetMovie("tt0381838");
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
            var html = File.ReadAllText(@"C:\GOT.txt");
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
                var genres = overview.SelectSingleNode(".//div[contains(@class, \"rec-cert-genre\")]");
                var directorsNode = overview.SelectSingleNode(".//div[@class=\"rec-director rec-ellipsis\"]");
                var actorsNode = overview.SelectSingleNode(".//div[@class=\"rec-actor rec-ellipsis\"]/span");
                var rateNode = overview.SelectSingleNode(".//div[@class=\"rating rating-list\"]/span[contains(@class, \"rating-rating\")]/span[@class=\"value\"]");
                //var rate = rateNode.SelectSingleNode("span[contains(@class, \"rating-rating\")]/span[@class=\"value\"]");

                if (directorsNode != null) directorsNode.RemoveChild(directorsNode.SelectSingleNode("b"));
                actorsNode.RemoveChild(actorsNode.SelectSingleNode("b"));

                foreach (var span in genres.SelectNodes("span"))
                {
                    genres.RemoveChild(span);
                }

                relatedMoviesList.Add(new Movie
                {
                    ImdbId = overview.Attribute("data-tconst"),
                    Title = overview.SelectSingleNode(".//div[@class=\"rec-title\"]/a/b").InnerTextClean(),
                    Year = overview.SelectSingleNode(".//div[@class=\"rec-title\"]/span").InnerTextClean().Replace("(", "").Replace(")", ""),
                    Poster = overview.SelectSingleNode(".//img[@class=\"loadlate rec_poster_img\"]").Attribute("src"),
                    Genre = genres.InnerText.CleanHtml().Replace("                                 ", "|"),
                    Summary = overview.SelectSingleNode(".//div[@class=\"rec-outline\"]/p").InnerTextClean(),
                    Directors = directorsNode != null ? directorsNode.InnerTextClean() : "",
                    Stars = actorsNode.InnerTextClean(),
                    Rate = Convert.ToDouble(rateNode.InnerText)
                });
            }

        }

        [TestMethod]
        public void GetImdbIdFromUrl()
        {
            var url = "http://www.imdb.com/title/tt1700844/?ref_=nv_sr_2";
            var splitted = url.Split('/');
        }

        [TestMethod]
        public void SearchImdbMovieByTitle()
        {
            var key = "Edge of Tomorrow";
            var scrapper = new ImdbScrapper(_xPathRepository);
            var movies = scrapper.SearchMovieByTitle(key);
        }

        [TestMethod]
        public void ReadJson()
        {
            var doc = new HtmlDocument();
            var html = File.ReadAllText(@"C:\js.txt");
            doc.LoadHtml(html);
            
            var script = doc.DocumentNode.SelectSingleNode("//script").InnerHtml;
            var startString = "window.IMDbReactInitialState.push({'mediaviewer':";
            var start = script.IndexOf(startString) + startString.Length;
            var end = script.LastIndexOf("});") - start;

            var json = script.Substring(start , end);
            var jObject = JObject.Parse(json);
            jObject.SelectToken("galleries.tt1975159.allImages[?(@id == 'rm3014517760')].src").ToString();
        }
    }
}
