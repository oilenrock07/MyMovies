using System.ComponentModel.DataAnnotations;

namespace MyMovies.Entities
{
    public class Review
    {
        [Key]
        public int ReviewId { get; set; }
        public int MovieId { get; set; }
        public string Reviews { get; set; }
    }
}
