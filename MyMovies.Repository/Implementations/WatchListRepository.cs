using MyMovies.Entities;
using MyMovies.Infrastructure.Interfaces;
using MyMovies.Infrastructure.Implementations;
using MyMovies.Repository.Interfaces;
using System.Collections.Generic;
using System.Linq;

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

        public IEnumerable<WatchList> GetTop10WatchList(string userId)
        {
            return Find(x => x.IsActive && x.UserId == userId).OrderByDescending(x => x.DateAdded).Take(10);
        }
    }
}
