using System.Net;
using System.Net.Http;
using System.Web.Http;
using MyMovies.Repository.Interfaces;
using MyMovies.Entities;
using MyMovies.Infrastructure.Interfaces;
using MyMovies.Web.BusinessLogic;
using Omu.ValueInjecter;

namespace MyMovies.Web.Controllers
{
    public class MoviesApiController : ApiController
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IMovieXPathRepository _movieXPathRepository;
        private readonly IUnitOfWork _unitOfWork;

        public MoviesApiController(IMovieRepository movieRepository, IMovieXPathRepository movieXPathRepository, IUnitOfWork unitOfWork)
        {
            _movieRepository = movieRepository;
            _movieXPathRepository = movieXPathRepository;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public Movie Get(string imdbId)
        {
            var movie = _movieRepository.GetByImdbId(imdbId);

            //Get from IMDB website
            if (movie == null)
            {
                var scrapper = new ImdbScrapper(_movieXPathRepository);
                movie = scrapper.GetMovie(imdbId);
            }

            return movie;
        }

        [HttpGet]
        public Movie Movie(int id)
        {
            var movie = _movieRepository.GetById(id);

            if (movie == null)
            {
                var resp = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent(string.Format("No movie with id ={0}", id)),
                    ReasonPhrase = "Movie not found"
                };
                throw new HttpResponseException(resp);
            }

            return movie;
        }

        [HttpPost]
        public void Post([FromBody]Movie movie)
        {
            _movieRepository.Add(movie);
            _unitOfWork.Commit();
        }

        [HttpPut]
        public void Put([FromBody]Movie movie)
        {
            var mov = _movieRepository.GetById(movie.MovieId);
            _movieRepository.Update(mov);
            mov.InjectFrom(movie);

            _unitOfWork.Commit();
        }

        [HttpDelete]
        public void Delete(int id)
        {
            var movie = _movieRepository.GetById(id);
            _movieRepository.Delete(movie);

            _unitOfWork.Commit();
        }
    }
}