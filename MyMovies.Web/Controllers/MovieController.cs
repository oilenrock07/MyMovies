using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using MyMovies.Common.Extension;
using MyMovies.Infrastructure.Interfaces;
using MyMovies.Repository.Interfaces;
using MyMovies.Web.ViewModels;
using Omu.ValueInjecter;
using MyMovies.Entities;
using MyMovies.Web.BusinessLogic;
using MyMovies.Web.Models;

namespace MyMovies.Web.Controllers
{
    public class MovieController : Controller
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IMovieXPathRepository _movieXPathRepository;
        private readonly IUnitOfWork _unitOfWork;

        public MovieController(IMovieRepository movieRepository, IMovieXPathRepository movieXPathRepository, IUnitOfWork unitOfWork)
        {
            _movieRepository = movieRepository;
            _movieXPathRepository = movieXPathRepository;
            _unitOfWork = unitOfWork;
        }

        public ActionResult Index(PaginationModel paginationViewModel = null)
        {
            var pagination = new Pagination();
            
            int count;
            IQueryable<Movie> movies;            
            if (paginationViewModel != null && !String.IsNullOrEmpty(paginationViewModel.Search))
            {
                var search = paginationViewModel.Search;

                //use String.Equals(row.Name, "test", StringComparison.OrdinalIgnoreCase)
                Expression<Func<Movie, bool>> expression = x => x.Title.Contains(search) || x.Stars.Contains(search) || x.Directors.Contains(search) || x.FileName.Contains(search) || x.ImdbId == search;
                movies = _movieRepository.Find(expression);
                count = movies.Count();
            }
            else if (paginationViewModel != null && !String.IsNullOrEmpty(paginationViewModel.Category))
            {
                movies = _movieRepository.Find(x => x.Genre.Contains(paginationViewModel.Category));
                count = movies.Count();
            }
            else if (paginationViewModel != null && !String.IsNullOrEmpty(paginationViewModel.Director))
            {
                movies = _movieRepository.Find(x => x.Directors.Contains(paginationViewModel.Director));
                count = movies.Count();
            }
            else if (paginationViewModel != null && !String.IsNullOrEmpty(paginationViewModel.Star))
            {
                movies = _movieRepository.Find(x => x.Stars.Contains(paginationViewModel.Star));
                count = movies.Count();
            }
            else if (paginationViewModel != null && !String.IsNullOrEmpty(paginationViewModel.Writer))
            {
                movies = _movieRepository.Find(x => x.Writers.Contains(paginationViewModel.Writer));
                count = movies.Count();
            }
            else
            {
                count = _movieRepository.GetAll().Count();
                movies = _movieRepository.GetAll();
            }

            var viewModel = new MoviePaginationViewModel();
            viewModel.Pagination = pagination.GetPaginationModel(Request, count);
            viewModel.ListTitle = GetListTitle(paginationViewModel);
            viewModel.Movies = pagination.TakePaginationModel(movies.OrderBy(x => x.Title).ToList(), viewModel.Pagination).MapCollection<MovieViewModel>();
            
            return View(viewModel);
        }

        public ActionResult Menu()
        {
            var categories = _movieRepository.GetGenres();
            return PartialView("_Menu", categories.ToArray());
        }

        public ActionResult Detail(int id)
        {
            var movie = _movieRepository.GetById(id);
            var viewModel = movie.MapItem<MovieViewModel>();

            if (!String.IsNullOrEmpty(movie.RelatedMovie))
            {
                viewModel.RelatedMovies = new List<Movie>();
                foreach (var item in movie.RelatedMovie.Split(','))
                {
                    var relatedMovie = _movieRepository.GetByImdbId(item);
                    if (relatedMovie != null)
                    {
                        viewModel.RelatedMovies.Add(relatedMovie);
                    }
                }
            }

            return View(viewModel);
        }

        private string GetListTitle(PaginationModel paginationViewModel)
        {
            if (paginationViewModel != null)
            {
                if (!String.IsNullOrEmpty(paginationViewModel.Search)) return String.Format("Search: {0}", paginationViewModel.Search);
                if (!String.IsNullOrEmpty(paginationViewModel.Category)) return String.Format("Category: {0}", paginationViewModel.Category);
                if (!String.IsNullOrEmpty(paginationViewModel.Star)) return String.Format("Actor: {0}", paginationViewModel.Star);
                if (!String.IsNullOrEmpty(paginationViewModel.Director)) return String.Format("Director: {0}", paginationViewModel.Director);
                if (!String.IsNullOrEmpty(paginationViewModel.Writer)) return String.Format("Writer: {0}", paginationViewModel.Writer);
            }

            return "";
        }
    }
}
