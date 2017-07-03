using MyMovies.Entities;
using MyMovies.Infrastructure.Interfaces;
using System.Collections.Generic;

namespace MyMovies.Repository.Interfaces
{
    public interface IWatchListRepository : IRepository<WatchList>
    {
        IEnumerable<WatchList> GetTop10WatchList(string userId);
    }
}
