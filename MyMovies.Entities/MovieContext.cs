using System.Data.Entity;

namespace MyMovies.Entities
{
    public class MovieContext : DbContext
    {
        public MovieContext():
            base("MovieConnectionString")
        {                
        }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<MovieXPath> MovieXPaths { get; set; }
    }
}
