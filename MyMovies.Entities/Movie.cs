using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyMovies.Entities
{
    public class Movie
    {
        [Key]
        public int MovieId { get; set; }

        [Required]
        public string ImdbId { get; set; }

        [Required]
        public virtual string Title { get; set; }
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

        public string Country { get; set; }
        public string Language { get; set; }
        public string AlsoKnownAs { get; set; }

        //Box Office
        public string Budget { get; set; }
        public string Gross { get; set; }

        //Custom
        public DateTime DateCreated { get; set; }
        public string Location { get; set; }
        public string Remarks { get; set; }

        [Required]
        public string FileName { get; set; }
        [Required]
        public string FileSize { get; set; }

        [NotMapped]
        public IList<Movie> RelatedMovies { get; set; }

        public Movie()
        {
            DateCreated = DateTime.Now;
            RelatedMovies = new List<Movie>();
        }
    }
}
