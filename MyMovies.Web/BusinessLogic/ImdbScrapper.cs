using HtmlAgilityPack;
using MyMovies.Entities;
using System;
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
            var poster = doc.DocumentNode.SelectSingleNode(xPath.Header).SelectSingleNode(xPath.Poster).Attribute("src");
            var stars = String.Join(",", doc.DocumentNode.SelectNodes(xPath.Stars).Select(x => x.SelectSingleNode("*/span").InnerTextClean()));
            var summary = doc.DocumentNode.SelectSingleNode(xPath.Summary).OuterHtmlClean();
            var year = doc.DocumentNode.SelectSingleNode(xPath.Year).InnerText();
            var genre = String.Join(",", doc.DocumentNode.SelectNodes(xPath.Genre).Select(x => x.InnerTextClean()));
            var runtime = doc.DocumentNode.SelectSingleNode(xPath.Runtime).InnerTextClean();
            var directors = doc.DocumentNode.SelectSingleNode(xPath.Header).SelectSingleNode(xPath.Directors).InnerTextClean();
            var writers = String.Join(",", doc.DocumentNode.SelectNodes(xPath.Writers).Select(x => x.SelectSingleNode("*/span").InnerTextClean()));
            var relatedMovies = doc.DocumentNode.SelectNodes(xPath.RelatedMovie);

            var strRelatedMovies = relatedMovies.Select(relatedMovieNode => String.Join(",", relatedMovieNode.SelectNodes("*/@data-tconst").Select(x => x.Attribute("data-tconst")))).ToList();

//cast: 
////*[@id=\"titleCast\"]/table//tr
//use select nodes

//sample node:
//          <td class="primary_photo">
//<a href="/name/nm3960408/?ref_=tt_cl_i15"><img height="44" width="32" alt="Magnus Nolan" title="Magnus Nolan" src="http://ia.media-imdb.com/images/G/01/imdb/images/nopicture/32x44/name-2138558783._CB282949327_.png" class=""></a>          </td>
//          <td class="itemprop" itemprop="actor" itemscope="" itemtype="http://schema.org/Person">
//<a href="/name/nm3960408/?ref_=tt_cl_t15" itemprop='url'> <span class="itemprop" itemprop="name">Magnus Nolan</span>
//</a>          </td>
//          <td class="ellipsis">
//              ...
//          </td>
//          <td class="character">
//              <div>
//            <a href="/character/ch0162707/?ref_=tt_cl_t15">James (20 months)</a> 
                  
//              </div>
//          </td>
      



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
                RelatedMovie = String.Join(",", strRelatedMovies)
            };

            return movie;
        }
    }
}