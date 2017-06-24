using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Xml;
using HtmlAgilityPack;
using MyMovies.Common.Extension;
using MyMovies.Entities;
using MyMovies.Repository.Interfaces;
using Newtonsoft.Json.Linq;

namespace MyMovies.Common.BusinessLogic
{
    public class ImdbScrapper
    {
        private readonly string _baseUrl = ConfigurationManager.AppSettings["ImdbBaseUrl"];
        private readonly string _searchTitleFormat = ConfigurationManager.AppSettings["ImdbSearchTitleFormat"];

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

            return movie;
        }

        public virtual Movie GetMovie(string imdbId)
        {
            var url = string.Format("{0}/title/{1}", _baseUrl, imdbId);
            var doc = _web.Load(url);
            
            var movie = MapResult(doc);
            movie.ImdbId = imdbId;
            
            return movie;
        }

        public virtual Movie GetMovie(string imdbId, out string document)
        {
            var url = string.Format("{0}/title/{1}", _baseUrl, imdbId);
            var doc = _web.Load(url);

            var movie = MapResult(doc);
            movie.ImdbId = imdbId;
            document = doc.DocumentNode.OuterHtml;

            return movie;
        }

        public virtual string GetMovieDocument(string imdbId)
        {
            var url = string.Format("{0}/title/{1}", _baseUrl, imdbId);
            var doc = _web.Load(url);

            return doc.DocumentNode.OuterHtml;           
        }

        public virtual IEnumerable<Movie> SearchMovieByTitle(string title)
        {
            var url = String.Format(_searchTitleFormat, title);
            var doc = _web.Load(url);
            //var doc = new HtmlDocument();
            //var html = File.ReadAllText(@"C:\search.txt");
            //doc.LoadHtml(html);

            var movieList = new List<Movie>();
            var result = doc.DocumentNode.SelectNodes("//*[contains(@class,'findResult')]");
            foreach (var item in result)
            {
                var urlLink = item.SelectSingleNode("//td//a").Attribute("href");
                var imdbId = urlLink.Split('/')[2];
                movieList.Add(new Movie
                {
                    Title = item.InnerText,
                    Poster = item.SelectSingleNode(".//td//img").Attribute("src"),
                    ImdbId = imdbId
                });
            }

            return movieList;
        }

        public virtual string GetHdImage(string url, string imdbId)
        {
            var doc = _web.Load(String.Format("{0}{1}", _baseUrl, url));
            var script = doc.DocumentNode.SelectSingleNode("//script").InnerHtml;
            const string startString = "window.IMDbReactInitialState.push({'mediaviewer':";

            var start = script.IndexOf(startString, StringComparison.Ordinal) + startString.Length;
            var end = script.LastIndexOf("});", StringComparison.Ordinal) - start;

            var json = script.Substring(start, end);
            var jObject = JObject.Parse(json);

            var imageId = url.Split('/').Last();
            if (imageId.Contains("?"))
                imageId = imageId.Split('?').First();
            var tokenQuery = String.Format("galleries.{0}.allImages[?(@id == '{1}')].src", imdbId, imageId);

            return jObject.SelectToken(tokenQuery).ToString();
        }

        protected Movie MapResult(HtmlDocument doc)
        {
            var xPath = _movieXPathRepository.GetXPath();
            if (xPath == null) throw new Exception("No XPathSetup");

            var title = doc.DocumentNode.SelectSingleNode(xPath.Title).OuterHtmlClean();
            var strRate = doc.DocumentNode.SelectSingleNode(xPath.Rate).InnerHtml();
            var rate = Convert.ToDouble(String.IsNullOrEmpty(strRate) ? "0" : strRate);
            var dateReleased = doc.DocumentNode.SelectSingleNode(xPath.DateReleased).InnerTextClean();
            var rating = doc.DocumentNode.SelectSingleNode(xPath.Rating).Attribute("content");
            
            var summary = doc.DocumentNode.SelectSingleNode(xPath.Summary).OuterHtmlClean();
            var year = doc.DocumentNode.SelectSingleNode(xPath.Year).InnerText();
            var genre = String.Join(", ", doc.DocumentNode.SelectNodes(xPath.Genre).Select(x => x.InnerTextClean()));
            var runtime = doc.DocumentNode.SelectSingleNode(xPath.Runtime).InnerTextClean();
            
            var stars = String.Join(",", doc.DocumentNode.SelectNodes(xPath.Stars).Select(x => x.SelectSingleNode("*/span").InnerTextClean()));
            var writers = doc.DocumentNode.SelectNodes(xPath.Writers) != null ? 
                          String.Join(",", doc.DocumentNode.SelectNodes(xPath.Writers).Select(x => x.SelectSingleNode("*/span").InnerTextClean()))
                          : "";
            var directors = doc.DocumentNode.SelectNodes(xPath.Directors) != null ?
                          String.Join(",", doc.DocumentNode.SelectNodes(xPath.Directors).Select(x => x.SelectSingleNode("*/span").InnerTextClean()))
                          : "";

            //using headers
            var header = doc.DocumentNode.SelectSingleNode(xPath.Header);
            var poster = header.SelectSingleNode(xPath.Poster).Attribute("src");
            var hdPosterLink = header.SelectSingleNode(xPath.Poster).ParentNode;
            var hdPosterUrl = "";
            if (hdPosterLink != null)
                hdPosterUrl = hdPosterLink.Attribute("href");

            var hdPoster = poster;
            //var directors = header.SelectSingleNode(xPath.Directors).InnerTextClean();

            //get the HD
            if (!String.IsNullOrEmpty(poster))
            {
                var splittedPosterUrl = poster.Split(new string[] { "@@" }, StringSplitOptions.None);
                if (splittedPosterUrl.Length > 1)
                    hdPoster = splittedPosterUrl[0] + "@@._V1_SY1000_CR0,0,676,1000_AL_.jpg";
            }

            var titleDetails = doc.DocumentNode.SelectSingleNode(xPath.TitleDetails).SelectNodes("*/h4");
            var country = titleDetails.FirstOrDefault(x => x.InnerText == "Country:").TitleDetailsAnchor();
            var language = titleDetails.FirstOrDefault(x => x.InnerText == "Language:").TitleDetailsAnchor();
            var alsoKnownAs = titleDetails.FirstOrDefault(x => x.InnerText == "Also Known As:").TitleDetailsText();
            var budget = titleDetails.FirstOrDefault(x => x.InnerText == "Budget:").TitleDetailsText();
            var gross = titleDetails.FirstOrDefault(x => x.InnerText == "Gross:").TitleDetailsText();

            //Related Movies
            var relatedMoviesList = new List<Movie>();
            var overviews = doc.DocumentNode.SelectNodes(xPath.RelatedRoot);
            if (overviews != null)
            {
                foreach (var overview in overviews)
                {
                    var genres = overview.SelectSingleNode(xPath.RelatedGenre);
                    var directorsNode = overview.SelectSingleNode(xPath.RelatedDirectors);
                    var actorsNode = overview.SelectSingleNode(xPath.RelatedStars);
                    var rateNode = overview.SelectSingleNode(xPath.RelatedRate);

                    if (directorsNode != null)
                    {
                        var directorNode = directorsNode.SelectSingleNode("b");
                        if (directorNode != null)
                            directorsNode.RemoveChild(directorsNode.SelectSingleNode("b"));
                    }

                    var actorNode = actorsNode.SelectSingleNode("b");
                    if (actorNode != null)
                        actorsNode.RemoveChild(actorsNode.SelectSingleNode("b"));

                    var genreSpans = genres.SelectNodes("span");
                    if (genreSpans != null)
                    {
                        foreach (var span in genreSpans)
                        {
                            genres.RemoveChild(span);
                        }
                    }

                    double relateMovieRate = 0;
                    var movieRate = rateNode != null ? Double.TryParse(rateNode.InnerText, out relateMovieRate) : false;
                    relatedMoviesList.Add(new Movie
                    {
                        ImdbId = overview.Attribute("data-tconst"),
                        Title = overview.SelectSingleNode(xPath.RelatedTitle).InnerTextClean(),
                        Year = overview.SelectSingleNode(xPath.RelatedYear).InnerTextClean().Replace("(", "").Replace(")", ""),
                        Poster = overview.SelectSingleNode(xPath.RelatedPoster).Attribute("src"),
                        Genre = genres.InnerText.CleanHtml().Replace("                                 ", "|"),
                        Summary = overview.SelectSingleNode(xPath.RelatedSummary).InnerTextClean(),
                        Directors = directorsNode != null ? directorsNode.InnerTextClean() : "",
                        Stars = actorsNode.InnerTextClean(),
                        Rate = movieRate ? relateMovieRate : 0
                    });
                }
            }
            
            var movie = new Movie
            {
                Title = title,
                Rate = rate,
                DateReleased = dateReleased,
                Rating = rating,
                OriginalPoster = poster,
                Poster = hdPoster,
                HdPosterLink = hdPosterUrl,
                Stars = stars,
                Summary = summary,
                Year = year,
                Genre = genre,
                Runtime = runtime,
                Directors = directors,
                Writers = writers,
                RelatedMovie = String.Join(",", relatedMoviesList.Select(x => x.ImdbId)), //String.Join(",", strRelatedMovies),
                Country = country,
                Language = language,
                AlsoKnownAs = alsoKnownAs,
                Budget = budget,
                Gross = gross,
                RelatedMovies = relatedMoviesList
            };

            return movie;
        }
    }
}