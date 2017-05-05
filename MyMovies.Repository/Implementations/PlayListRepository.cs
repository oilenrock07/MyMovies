using MyMovies.Entities;
using MyMovies.Infrastructure.Interfaces;
using MyMovies.Infrastructure.Implementations;
using MyMovies.Repository.Interfaces;

namespace MyMovies.Repository.Implementations
{
    public class PlayListRepository : Repository<PlayList>, IPlayListRepository
    {
        private readonly IDatabaseFactory _databaseFactory;

        public PlayListRepository(IDatabaseFactory databaseFactory)
            : base (databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }
    }
}
