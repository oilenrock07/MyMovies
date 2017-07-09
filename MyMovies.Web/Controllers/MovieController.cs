using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
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
        private readonly IBannerRepository _bannerRepository;
        private readonly IWatchListRepository _watchListRepository;

        public MovieController(IMovieRepository movieRepository, IMovieXPathRepository movieXPathRepository, IUnitOfWork unitOfWork, IBannerRepository bannerRepository,
            IWatchListRepository watchListRepository)
        {
            _movieRepository = movieRepository;
            _movieXPathRepository = movieXPathRepository;
            _unitOfWork = unitOfWork;
            _bannerRepository = bannerRepository;
            _watchListRepository = watchListRepository;
        }

        public ActionResult Index(PaginationModel paginationViewModel = null)
        {                       
            int count;
            var movies = FilterMovie(paginationViewModel, out count);

            var pagination = new Pagination();
            var viewModel = new MoviePaginationViewModel();
            ViewBag.MovieList = "true";
            viewModel.Banner = LoadBanner(paginationViewModel);
            viewModel.Pagination = pagination.GetPaginationModel(GetPageNumber(), count);

            var order = GetMovieOrderList();
            var sorting = Request.Cookies["MovieSorting"];

            var moviePagination = (sorting != null && sorting.Value == "Desc") ? 
                                  pagination.TakePaginationModel(movies.OrderByDescending(order).ToList(), viewModel.Pagination) :
                                  pagination.TakePaginationModel(movies.OrderBy(order).ToList(), viewModel.Pagination);
                            
            viewModel.Movies = moviePagination.MapCollection<MovieViewModel>();
            MapPaginationFilters(paginationViewModel, viewModel.Pagination);

            return View(viewModel);
        }

        public ActionResult Menu()
        {
            //load banners
            var banners = _bannerRepository.Find(x => !x.IsDeleted);
            var categories = _movieRepository.GetGenres();

            var model = new List<Tuple<string, string>>();
            foreach (var category in categories)
            {
                var banner = banners.FirstOrDefault(x => x.Identifier == category);
                var poster = banner != null ? banner.Poster : "";
                model.Add(new Tuple<string, string>(category, poster));
            }

            return PartialView("_Menu", model);
        }

        public ActionResult Detail(int id)
        {
            var movie = _movieRepository.GetById(id);
            var viewModel = movie.MapItem<MovieViewModel>();

            if (!String.IsNullOrEmpty(movie.RelatedMovie))
            {
                viewModel.RelatedMoviesViewModel = new List<MovieViewModel>();
                foreach (var item in movie.RelatedMovie.Split(','))
                {
                    var relatedMovie = _movieRepository.GetByImdbId(item);
                    if (relatedMovie != null)
                    {
                        viewModel.RelatedMoviesViewModel.Add(relatedMovie.MapItem<MovieViewModel>());
                    }
                }
            }

            return View(viewModel);
        }

        public ActionResult Random()
        {
            //Get the ids
            var movieIds = _movieRepository.GetAllMovieIds().ToList();
            if (movieIds.Any())
            {
                var randomIds = new List<int>(50);

                while (true)
                {
                    var rnd = new Random();
                    var randomId = rnd.Next(1, movieIds.Count);

                    var movieId = movieIds.ElementAt(randomId - 1);
                    if (!randomIds.Contains(movieId))
                    {
                        randomIds.Add(movieId);
                        if (randomIds.Count == 50 || randomIds.Count == movieIds.Count)
                            break;
                    }
                }

                var movies = _movieRepository.GetAllMoviesByMovieId(randomIds);
                var viewModel = movies.MapCollection<MovieViewModel>();
                return View(viewModel);
            }

            return View();
        }

        //[Authorize]
        public JsonResult AddToWatchList(int movieId)
        {
            var userId = User.Identity.GetUserId();
            var watchList = new WatchList
            {
                UserId = userId,
                IsActive = true,
                MovieId = movieId
            };

            _watchListRepository.Add(watchList);
            _unitOfWork.Commit();

            //return the top 10 watchlist
            var top10WatchList = _watchListRepository.GetTop10WatchList(userId);
            return Json(top10WatchList);
        }

        //[Authorize]
        public void RemoveToWatchList(int movieId)
        {
            var watchList = _watchListRepository.Find(x => x.MovieId == movieId && x.UserId == User.Identity.GetUserId());
            if (watchList != null)
            {
                var movieToWatch = watchList.First();
                movieToWatch.IsActive = false;
                _unitOfWork.Commit();
            }
        }

        #region Private Methods

        private Banner LoadBanner(PaginationModel paginationViewModel)
        {
            if (paginationViewModel != null)
            {
                var identifier = "";
                if (!String.IsNullOrEmpty(paginationViewModel.Category))
                    identifier = paginationViewModel.Category;
                else if (!String.IsNullOrEmpty(paginationViewModel.Star))
                    identifier = paginationViewModel.Star;
                else if (!String.IsNullOrEmpty(paginationViewModel.Director))
                    identifier = paginationViewModel.Director;
                else if (!String.IsNullOrEmpty(paginationViewModel.Writer))
                    identifier = paginationViewModel.Writer;

                var banner = _bannerRepository.Find(x => x.Identifier == identifier && !x.IsDeleted);
                if (banner != null && banner.Any())
                    return banner.First();
            }

            return new Banner {Poster = "/Images/default.jpg", TextColor = "white"};
        }

        private void MapPaginationFilters(PaginationModel requestPagination, PaginationModel newPagination)
        {
            if (requestPagination == null) return;

            newPagination.Category = requestPagination.Category;
            newPagination.Search = requestPagination.Search;
            newPagination.Star = requestPagination.Star;
            newPagination.Director = requestPagination.Director;
            newPagination.Writer = requestPagination.Writer;
        }

        private int GetPageNumber()
        {
            return Request.QueryString["Page"] != null ? Convert.ToInt32(Request.QueryString["Page"]) :
                   RouteData.Values.ContainsKey("Page") ? Convert.ToInt32(RouteData.Values["Page"]) : 1;
        }

        private IQueryable<Movie> FilterMovie(PaginationModel paginationViewModel, out int count)
        {
            IQueryable<Movie> movies;
            if (paginationViewModel != null && !String.IsNullOrEmpty(paginationViewModel.Search))
            {
                var search = paginationViewModel.Search.ToLower();

                //use String.Equals(row.Name, "test", StringComparison.OrdinalIgnoreCase)
                Expression<Func<Movie, bool>> expression = x => x.Title.ToLower().Contains(search) || x.Stars.ToLower().Contains(search) || x.Directors.ToLower().Contains(search) || x.FileName.ToLower().Contains(search) || x.AlsoKnownAs.ToLower().Contains(search) || x.ImdbId == search;
                movies = _movieRepository.Find(expression);
                count = movies.Count();
                ViewBag.Search = paginationViewModel.Search;
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

            return movies;
        }

        private Func<Movie, object> GetMovieOrderList()
        {
            var order = Request.Cookies["MovieOrder"];
            if (order != null)
            {
                switch (order.Value)
                {
                    case "Rate":
                        return x => x.Rate;
                    case "Year":
                        return x => x.Year;
                    case "DateAdded":
                        return x => x.DateCreated;
                }
            }

            return x => x.Title;
        }
        #endregion
        
    }
}
