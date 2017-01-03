using System.ComponentModel.DataAnnotations;

namespace MyMovies.Entities
{
    public class MovieXPath
    {
        [Key]
        public int MovieXPathId { get; set; }

        public string Title { get; set; }
        public string Year { get; set; }
        public string Rate { get; set; }
        public string Runtime { get; set; }
        public string Rating { get; set; }
        public string DateReleased { get; set; }
        public string Summary { get; set; }
        public string Genre { get; set; }
        public string Duration { get; set; }
        //public string RelatedMovie { get; set; }

        public string TitleDetails { get; set; }
        public string Header { get; set; }

        public string Poster { get; set; }
        public string Directors { get; set; }
        public string Writers { get; set; }
        public string Stars { get; set; }

        public string Country { get; set; }
        public string Language { get; set; }
        public string AlsoKnownAs { get; set; }

        //Box Office
        public string Budget { get; set; }
        public string Gross { get; set; }

        //Related Movies
        public string RelatedRoot { get; set; }
        public string RelatedDirectors { get; set; }
        public string RelatedStars { get; set; }
        public string RelatedRate { get; set; }
        public string RelatedTitle { get; set; }
        public string RelatedYear { get; set; }
        public string RelatedPoster { get; set; }
        public string RelatedGenre { get; set; }
        public string RelatedSummary { get; set; }
    }
}
