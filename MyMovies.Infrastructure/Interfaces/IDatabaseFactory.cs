using MyMovies.Entities;

namespace MyMovies.Infrastructure.Interfaces
{
    public interface IDatabaseFactory
    {
        MovieContext GetContext();
    }
}
