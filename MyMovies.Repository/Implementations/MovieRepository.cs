using MyMovies.Entities;
using MyMovies.Infrastructure.Implementations;
using MyMovies.Infrastructure.Interfaces;
using MyMovies.Repository.Interfaces;

namespace MyMovies.Repository.Implementations
{
    public class MovieRepository : Repository<Movie> , IMovieRepository
    {
        public MovieRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
                
        }
    }
}
