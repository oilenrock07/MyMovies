using HtmlAgilityPack;
using MyMovies.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
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

        public Movie GetMovie(string imdbId)
        {
            var xPath = _movieXPathRepository.GetXPath();
            if (xPath == null) throw new Exception("No XPathSetup");

            var url = string.Format("{0}{1}", _baseUrl, imdbId);
            var doc = _web.Load(url);

            var movie = new Movie
            {
                Title = doc.DocumentNode.SelectSingleNode(xPath.Title).OuterHtml.CleanHtml(),
                Rate = Convert.ToDouble(doc.DocumentNode.SelectSingleNode(xPath.Rate).InnerHtml),
                DateReleased = doc.DocumentNode.SelectSingleNode(xPath.DateReleased).InnerText.CleanHtml(),
                Rating = doc.DocumentNode.SelectSingleNode(xPath.Rating).Attributes["content"].Value,
                Poster = (doc.DocumentNode.SelectSingleNode(xPath.Poster) ?? doc.DocumentNode.SelectSingleNode(xPath.Poster2)).Attributes["src"].Value,
                Stars = String.Join("", (doc.DocumentNode.SelectNodes(xPath.Stars) ?? doc.DocumentNode.SelectNodes(xPath.Stars2)).Select(x => x.InnerText.CleanHtml()).Where(x => x != "|")),
                Summary = doc.DocumentNode.SelectSingleNode(xPath.Summary).OuterHtml.CleanHtml(),
                Year = doc.DocumentNode.SelectSingleNode(xPath.Year).InnerText,
                Genre = String.Join(",", doc.DocumentNode.SelectNodes(xPath.Genre).Select(x => x.InnerText.CleanHtml())),
                Runtime = doc.DocumentNode.SelectSingleNode(xPath.Runtime).InnerText.CleanHtml(),
                Directors = (doc.DocumentNode.SelectSingleNode(xPath.Directors) ?? doc.DocumentNode.SelectSingleNode(xPath.Directors2)).InnerText,
            };

            return movie;
        }
    }
}