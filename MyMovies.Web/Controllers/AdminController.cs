using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyMovies.Common.Extension;
using MyMovies.Entities;
using MyMovies.Infrastructure.Interfaces;
using MyMovies.Repository.Interfaces;
using MyMovies.Web.BusinessLogic;
using MyMovies.Web.ViewModels;
using Omu.ValueInjecter;

namespace MyMovies.Web.Controllers
{
    public class AdminController : Controller
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IMovieXPathRepository _movieXPathRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AdminController(IMovieRepository movieRepository, IUnitOfWork unitOfWork,IMovieXPathRepository movieXPathRepository)
        {
            _movieRepository = movieRepository;
            _unitOfWork = unitOfWork;
            _movieXPathRepository = movieXPathRepository;
        }

        public ActionResult Index(int id = 0)
        {
            var viewModel = new MovieViewModel();
            if (id > 0)
            {
                var movie = _movieRepository.GetById(id);
                viewModel = movie.MapItem<MovieViewModel>();
            }

            return View(viewModel);
        }

        public ActionResult Search(string key)
        {
            Movie movie;
            if (key.IsIMDB())
            {
                //search for local record
                movie = _movieRepository.GetByImdbId(key);
                if (movie == null)
                {
                    var scrapper = new ImdbScrapper(_movieXPathRepository);
                    movie = scrapper.GetMovie(key);
                }
            }
            else
            {
                movie = _movieRepository.GetByMovieName(key);
            }

            ViewBag.Search = key;
            var viewModel = movie.MapItem<MovieViewModel>();
            return View("Index", viewModel);
        }

        [HttpPost]
        public ActionResult Index(MovieViewModel viewModel)
        {
            var id = viewModel.MovieId;
            try
            {                
                if (viewModel.MovieId == 0)
                {
                    var movie = viewModel.MapItem<Movie>();
                    _movieRepository.Add(movie);
                    _unitOfWork.Commit();
                    id = movie.MovieId;

                    TempData["Save"] = MyMovieResource.MoviedAdded;
                }
                else
                {
                    var movie = _movieRepository.GetById(viewModel.MovieId);
                    _movieRepository.Update(movie);
                    movie.InjectFrom(viewModel);

                    _unitOfWork.Commit();

                    TempData["Save"] = MyMovieResource.MovieUpdated;
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = MyMovieResource.MovieErrorSaving;
                return View("Index", viewModel);
            }

            return RedirectToAction("Index", new { id });
        }
    }
}
