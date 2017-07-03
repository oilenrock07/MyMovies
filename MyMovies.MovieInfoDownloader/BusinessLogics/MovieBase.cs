using MyMovies.Common.BusinessLogic;
using MyMovies.Entities;
using MyMovies.Infrastructure.Implementations;
using MyMovies.Repository.Implementations;

namespace MyMovies.MovieInfoDownloader.BusinessLogics
{
    public abstract class MovieBase
    {
        protected readonly DatabaseFactory _databaseFactory;
        protected readonly UnitOfWork _unitOfWork;
        protected readonly MovieXPathRepository _movieXPathRepository;
        protected readonly MovieRepository _movieRepository;
        protected readonly ImdbScrapper _imdbScrapper;

        public MovieBase()
        {
            _databaseFactory = new DatabaseFactory(new MovieContext());
            _unitOfWork = new UnitOfWork(_databaseFactory);
            _movieXPathRepository = new MovieXPathRepository(_databaseFactory, null);
            _movieRepository = new MovieRepository(_databaseFactory);
            _imdbScrapper = new ImdbScrapper(_movieXPathRepository);
        }
    }
}
