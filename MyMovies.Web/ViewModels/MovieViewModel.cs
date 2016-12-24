using MyMovies.Entities;
using System.Collections.Generic;
using System.Linq;

namespace MyMovies.Web.ViewModels
{
    public class MovieViewModel : Movie
    {
        public IEnumerable<string> StarLists
        {
            get
            {
                return Stars.Split(',').Select(x => x.Trim());
            }
        }

        public IEnumerable<string> GenreList
        {
            get
            {
                return Genre.Split(',').Select(x => x.Trim());
            } 
        } 

        public IList<Movie> RelatedMovies { get; set; }

        public string FormattedRate
        {
            get
            {
                return Rate.ToString("#.#");
            }
        }
    }
}