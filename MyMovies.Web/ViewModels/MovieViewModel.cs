using System;
using MyMovies.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyMovies.Web.ViewModels
{
    public class MovieViewModel : Movie
    {
        [AllowHtml]
        public override string Title { get; set; }

        public string DisplayTitle
        {
            get { return (String.IsNullOrEmpty(Year)) ? Title : String.Format("{0} ({1})", Title, Year); }
        }

        public string DisplayRate
        {
            get { return (Rate%1) == 0 ? Rate.ToString() : Rate.ToString("#.0"); }
        }

        public string LastStar
        {
            get { return StarLists.LastOrDefault(); }
        }
        public IEnumerable<string> StarLists
        {
            get
            {
                if (Stars == null)
                    return new List<string>();
                return Stars.Split(',').Select(x => x.Trim());
            }
        }

        public string LastGenre
        {
            get { return GenreList.LastOrDefault(); }
        }

        public IEnumerable<string> GenreList
        {
            get
            {
                if (Genre == null)
                    return new List<string>();
                return Genre.Split(',').Select(x => x.Trim());
            } 
        }

        public string LastDirector
        {
            get { return DirectorList.LastOrDefault(); }
        }

        public IEnumerable<string> DirectorList
        {
            get
            {
                if (Directors == null)
                    return new List<string>();

                if (Directors.Contains(","))
                    return Directors.Split(',').Select(x => x.Trim());

                return new [] { Directors};
            }
        }

        public string LastWriter
        {
            get { return WriterList.LastOrDefault(); }
        }

        public IEnumerable<string> WriterList
        {
            get
            {
                if (Writers == null)
                    return new List<string>();
                return Writers.Split(',').Select(x => x.Trim());
            }
        } 

        public string FormattedRate
        {
            get
            {
                return Rate.ToString("#.#");
            }
        }

        public string ShortenedSumary
        {
            get
            {
                if (String.IsNullOrEmpty(Summary)) return "";
                return Summary.Length > 250 ? String.Format("{0}...", Summary.Substring(0, 250)) : Summary;
            }
        }

        public bool UpdateImage { get; set; }
        public bool UseOriginalPoster { get; set; }
        public HttpPostedFileBase MoviePoster { get; set; }
        public IList<MovieViewModel> RelatedMoviesViewModel { get; set; }
        public IEnumerable<WatchList> WatchList { get; set; }

        public bool IsOnWatchList()
        {
            if (WatchList != null && WatchList.Any())
                return WatchList.FirstOrDefault(x => x.MovieId == MovieId) != null;

            return false;
        }
    }
}