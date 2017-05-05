using System.ComponentModel.DataAnnotations;

namespace MyMovies.Entities
{
    public class MoviePlayList
    {
        [Key]
        public int MoviePlayListId { get; set; }

        public int PlayListId { get; set; }

        public int MovieId { get; set; }
    }
}
