using System;
using System.ComponentModel.DataAnnotations;

namespace MyMovies.Entities
{
    public class WatchList
    {
        [Key]
        public int WatchListId { get; set; }

        public int UserId { get; set; }

        public int MovieId { get; set; }

        public bool IsActive { get; set; }

        public DateTime DateAdded { get; set; }
    }
}
