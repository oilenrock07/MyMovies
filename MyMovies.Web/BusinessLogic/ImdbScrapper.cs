using HtmlAgilityPack;
using MyMovies.Entities;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using MyMovies.Common.Extension;
using MyMovies.Repository.Interfaces;

namespace MyMovies.Web.BusinessLogic
{
    public class ImdbScrapper
    {
        private readonly string _baseUrl = ConfigurationManager.AppSettings["ImdbBaseUrl"];
        private readonly HtmlWeb _web = new HtmlWeb();
        private readonly IMovieXPathRepository _movieXPathRepository;

        public ImdbScrapper(IMovieXPathRepository movieXPathRepository)
        {
            _movieXPathRepository = movieXPathRepository;
        }

        public virtual Movie LoadMovieFromFile(string fileName)
        {
            var doc = new HtmlDocument();
            var html = File.ReadAllText(fileName);
            doc.LoadHtml(html);

            var movie = MapResult(doc);

            GC.Collect();
            return movie;
        }

        public virtual Movie GetMovie(string imdbId)
        {
            var url = string.Format("{0}{1}", _baseUrl, imdbId);
            var doc = _web.Load(url);

            var movie = MapResult(doc);
            movie.ImdbId = imdbId;

            GC.Collect();
            return movie;
        }

        protected Movie MapResult(HtmlDocument doc)
        {
            var xPath = _movieXPathRepository.GetXPath();
            if (xPath == null) throw new Exception("No XPathSetup");

            var title = doc.DocumentNode.SelectSingleNode(xPath.Title).OuterHtmlClean();
            var rate = Convert.ToDouble(doc.DocumentNode.SelectSingleNode(xPath.Rate).InnerHtml());
            var dateReleased = doc.DocumentNode.SelectSingleNode(xPath.DateReleased).InnerTextClean();
            var rating = doc.DocumentNode.SelectSingleNode(xPath.Rating).Attribute("content");
            
            var summary = doc.DocumentNode.SelectSingleNode(xPath.Summary).OuterHtmlClean();
            var year = doc.DocumentNode.SelectSingleNode(xPath.Year).InnerText();
            var genre = String.Join(",", doc.DocumentNode.SelectNodes(xPath.Genre).Select(x => x.InnerTextClean()));
            var runtime = doc.DocumentNode.SelectSingleNode(xPath.Runtime).InnerTextClean();
            
            var stars = String.Join(",", doc.DocumentNode.SelectNodes(xPath.Stars).Select(x => x.SelectSingleNode("*/span").InnerTextClean()));
            var writers = String.Join(",", doc.DocumentNode.SelectNodes(xPath.Writers).Select(x => x.SelectSingleNode("*/span").InnerTextClean()));
            
            //using headers
            var header = doc.DocumentNode.SelectSingleNode(xPath.Header);
            var poster = header.SelectSingleNode(xPath.Poster).Attribute("src");
            var directors = header.SelectSingleNode(xPath.Directors).InnerTextClean();

            var relatedMovies = doc.DocumentNode.SelectNodes(xPath.RelatedMovie);
            var strRelatedMovies = relatedMovies.Select(relatedMovieNode => String.Join(",", relatedMovieNode.SelectNodes("*/@data-tconst").Select(x => x.Attribute("data-tconst")))).ToList();

            var titleDetails = doc.DocumentNode.SelectSingleNode(xPath.TitleDetails).SelectNodes("*/h4");
            var country = titleDetails.FirstOrDefault(x => x.InnerText == "Country:").TitleDetailsAnchor();
            var language = titleDetails.FirstOrDefault(x => x.InnerText == "Language:").TitleDetailsAnchor();
            var alsoKnownAs = titleDetails.FirstOrDefault(x => x.InnerText == "Also Known As:").TitleDetailsText();
            var budget = titleDetails.FirstOrDefault(x => x.InnerText == "Budget:").TitleDetailsText();
            var gross = titleDetails.FirstOrDefault(x => x.InnerText == "Gross:").TitleDetailsText();

            var movie = new Movie
            {
                Title = title,
                Rate = rate,
                DateReleased = dateReleased,
                Rating = rating,
                Poster = poster,
                Stars = stars,
                Summary = summary,
                Year = year,
                Genre = genre,
                Runtime = runtime,
                Directors = directors,
                Writers = writers,
                RelatedMovie = String.Join(",", strRelatedMovies),
                Country = country,
                Language = language,
                AlsoKnownAs = alsoKnownAs,
                Budget = budget,
                Gross = gross
            };

            return movie;
        }
    }
}