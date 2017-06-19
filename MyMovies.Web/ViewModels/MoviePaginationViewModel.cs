using System.Collections.Generic;
using MyMovies.Entities;
using MyMovies.Web.Models;

namespace MyMovies.Web.ViewModels
{
    public class MoviePaginationViewModel
    {
        public Banner Banner { get; set; }
        public PaginationModel Pagination { get; set; }
        public IEnumerable<MovieViewModel> Movies { get; set; }
    }
}