using System.ComponentModel.DataAnnotations;

namespace MyMovies.Entities
{
    public class Movie
    {
        [Key]
        public int MovieId { get; set; }

        public string ImdbId { get; set; }
        public string Title { get; set; }
        public string Year { get; set; }
        public double Rate { get; set; }
        public string Runtime { get; set; }
        public string Rating { get; set; }
        public string DateReleased { get; set; }
        public string Poster { get; set; }
        public string Directors { get; set; }
        public string Writers { get; set; }
        public string Stars { get; set; }
        public string Summary { get; set; }
        public string Genre { get; set; }
        public string RelatedMovie { get; set; }
    }
}
