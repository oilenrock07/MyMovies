using MyMovies.Entities;
using MyMovies.Infrastructure.Interfaces;

namespace MyMovies.Repository.Interfaces
{
    public interface IMovieXPathRepository : IRepository<MovieXPath>
    {
        MovieXPath GetXPath();
    }
}
