using System;
using System.Collections.Generic;
using System.Linq;
using CacheManager.Core;
using MyMovies.Entities;
using MyMovies.Infrastructure.Implementations;
using MyMovies.Infrastructure.Interfaces;
using MyMovies.Repository.Interfaces;

namespace MyMovies.Repository.Implementations
{
    public class MovieRepository : Repository<Movie> , IMovieRepository
    {
        private readonly ICacheManager<object> _cacheManager;

        public MovieRepository(IDatabaseFactory databaseFactory, ICacheManager<object> cacheManager = null)
            : base(databaseFactory)
        {
            _cacheManager = cacheManager;
        }

        public virtual Movie GetByImdbId(string imdbId)
        {
            return Find(x => x.ImdbId == imdbId).FirstOrDefault();
        }

        public virtual Movie GetByMovieName(string movieName)
        {
            return Find(x => x.Title == movieName).FirstOrDefault();
        }

        public virtual Movie GetMovieByAKA(string movieName)
        {
            return Find(x => x.AlsoKnownAs == movieName).FirstOrDefault();
        }

        public virtual IEnumerable<string> GetGenres()
        {
            var cachedGenre = _cacheManager != null ? _cacheManager.Get("Genres") : null;
            if (cachedGenre == null)
            {
                var genres = GetAll().Select(x => x.Genre).ToList();
                if (_cacheManager != null)
                {
                    var splittedGenre = genres.SelectMany(x => x.Split(new[]{", "}, StringSplitOptions.None))
                                       .Select(x => x.Trim()).Distinct().OrderBy(x => x);
                    genres = splittedGenre.ToList();
                    _cacheManager.Add("Genres", splittedGenre);
                }

                return genres;
            }

            return cachedGenre as IEnumerable<string>;
        }
    }
}
