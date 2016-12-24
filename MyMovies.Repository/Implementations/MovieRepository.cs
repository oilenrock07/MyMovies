﻿using System.Linq;
using MyMovies.Entities;
using MyMovies.Infrastructure.Implementations;
using MyMovies.Infrastructure.Interfaces;
using MyMovies.Repository.Interfaces;

namespace MyMovies.Repository.Implementations
{
    public class MovieRepository : Repository<Movie> , IMovieRepository
    {
        public MovieRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
                
        }

        public virtual Movie GetByImdbId(string imdbId)
        {
            return Find(x => x.ImdbId == imdbId).FirstOrDefault();
        }

        public virtual Movie GetByMovieName(string movieName)
        {
            return Find(x => x.Title == movieName).FirstOrDefault();
        }
    }
}
