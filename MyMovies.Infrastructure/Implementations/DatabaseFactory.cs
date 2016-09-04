using MyMovies.Entities;
using MyMovies.Infrastructure.Interfaces;

namespace MyMovies.Infrastructure.Implementations
{
    public class DatabaseFactory : IDatabaseFactory
    {
        private MovieContext _context;

        public DatabaseFactory()
        {
        }

        public DatabaseFactory(MovieContext context)
        {
            _context = context;
        }

        public virtual MovieContext GetContext()
        {
            if (_context != null) return _context;

            _context = new MovieContext();
            return _context;
        }
    }
}
