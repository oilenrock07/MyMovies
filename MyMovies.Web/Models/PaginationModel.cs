using System;

namespace MyMovies.Web.Models
{
    public class PaginationModel
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int TotalItems { get; set; }
        public string PageName { get; set; }
        public int ItemsPerPage { get; set; }
        public int DefaultItemsPerPage { get; set; }
        public string PagingText { get; set; }

        public string Search { get; set; }
        public string Category { get; set; }
        public string Star { get; set; }
        public string Director { get; set; }
        public string Writer { get; set; }
    }
}