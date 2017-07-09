using System.Collections.Generic;
using MyMovies.Entities;
using MyMovies.Infrastructure.Interfaces;

namespace MyMovies.Repository.Interfaces
{
    public interface IMovieRepository : IRepository<Movie>
    {
        Movie GetByImdbId(string imdbId);
        Movie GetByMovieName(string movieName);
        Movie GetMovieByAKA(string movieName);
        IEnumerable<string> GetGenres();
        IEnumerable<int> GetAllMovieIds();
        IEnumerable<Movie> GetAllMoviesByMovieId(IEnumerable<int> movieIds);
    }
}
