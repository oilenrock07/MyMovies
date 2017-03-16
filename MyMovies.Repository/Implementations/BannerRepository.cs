using MyMovies.Entities;
using MyMovies.Infrastructure.Interfaces;
using MyMovies.Infrastructure.Implementations;
using MyMovies.Repository.Interfaces;

namespace MyMovies.Repository.Implementations
{
    public class BannerRepository : Repository<Banner>, IBannerRepository
    {
        private readonly IDatabaseFactory _databaseFactory;

        public BannerRepository(IDatabaseFactory databaseFactory)
            : base (databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }
    }
}
