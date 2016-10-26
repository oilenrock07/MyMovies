using MyMovies.Entities;
using System.Collections.Generic;
using System.Linq;

namespace MyMovies.Web.ViewModels
{
    public class MovieViewModel : Movie
    {
        public IEnumerable<string> StarLists => Stars.Split(',').Select(x => x.Trim());
        public IEnumerable<string> GenreList => Genre.Split(',').Select(x => x.Trim());
        public IList<Movie> RelatedMovies { get; set; }

        public string FormattedRate => Rate.ToString("#.#");
    }
}