using MyMovies.Entities;
using MyMovies.Infrastructure.Interfaces;
using MyMovies.Infrastructure.Implementations;
using MyMovies.Repository.Interfaces;

namespace MyMovies.Repository.Implementations
{
    public class WatchListRepository : Repository<WatchList>, IWatchListRepository
    {
        private readonly IDatabaseFactory _databaseFactory;

        public WatchListRepository(IDatabaseFactory databaseFactory)
            : base (databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }
    }
}
