using System.Collections.Generic;
using MyMovies.Web.Models;

namespace MyMovies.Web.ViewModels
{
    public class MoviePaginationViewModel
    {
        public string Banner { get; set; }
        public PaginationModel Pagination { get; set; }
        public IEnumerable<MovieViewModel> Movies { get; set; }
    }
}