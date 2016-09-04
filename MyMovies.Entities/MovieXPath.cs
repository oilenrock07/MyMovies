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
        public string RelatedMovie { get; set; }

        //#2 will be used when the movie has trailer (structure of the html page is different)
        public string Poster { get; set; }
        public string Poster2 { get; set; } 
        
        public string Directors { get; set; }
        public string Directors2 { get; set; } 

        public string Writers { get; set; }
        public string Writers2 { get; set; }

        public string Stars { get; set; }
        public string Stars2 { get; set; }
    }
}
