using System.Linq;
using CacheManager.Core;
using MyMovies.Entities;
using MyMovies.Infrastructure.Implementations;
using MyMovies.Infrastructure.Interfaces;
using MyMovies.Repository.Interfaces;

namespace MyMovies.Repository.Implementations
{
    public class MovieXPathRepository : Repository<MovieXPath> , IMovieXPathRepository
    {
        private readonly ICacheManager<object> _cacheManager;

        public MovieXPathRepository(IDatabaseFactory databaseFactory, ICacheManager<object> cacheManager)
            : base(databaseFactory)
        {
            _cacheManager = cacheManager;
        }

        public virtual MovieXPath GetXPath()
        {
            var cachedMovieXPath = _cacheManager != null ? _cacheManager.Get("MovieXPaths") : null;
            if (cachedMovieXPath == null)
            {
                var xPath = GetAll().Last();
                if (_cacheManager != null) _cacheManager.Add("MovieXPaths", xPath);

                return xPath;
            }

            return cachedMovieXPath as MovieXPath;
        }
    }
}
