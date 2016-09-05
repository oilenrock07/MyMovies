using MyMovies.Entities;
using MyMovies.Infrastructure.Interfaces;

namespace MyMovies.Repository.Interfaces
{
    public interface IMovieRepository : IRepository<Movie>
    {
        Movie GetByImdbId(string imdbId);
    }
}
