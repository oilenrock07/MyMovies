using System;
using System.Web.Mvc;
using MyMovies.Common.BusinessLogic;
using MyMovies.Common.Extension;
using MyMovies.Entities;
using MyMovies.Infrastructure.Interfaces;
using MyMovies.Repository.Interfaces;
using MyMovies.Web.ViewModels;
using Omu.ValueInjecter;
using System.Net;

namespace MyMovies.Web.Controllers
{
    public class AdminController : Controller
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IMovieXPathRepository _movieXPathRepository;
        private readonly IBannerRepository _bannerRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AdminController(IMovieRepository movieRepository, IUnitOfWork unitOfWork,IMovieXPathRepository movieXPathRepository, IBannerRepository bannerRepository)
        {
            _movieRepository = movieRepository;
            _bannerRepository = bannerRepository;
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
                    //movie = scrapper.LoadMovieFromFile("C:/GOT.txt");
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
                UpdateImage(viewModel);

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

        [HttpGet]
        public ActionResult MovieUpdate(int id)
        {
            var viewModel = new MovieViewModel();
            if (id > 0)
            {
                var movie = _movieRepository.GetById(id);
                var scrapper = new ImdbScrapper(_movieXPathRepository);
                var scrappedMovie = scrapper.GetMovie(movie.ImdbId);

                viewModel = scrappedMovie.MapItem<MovieViewModel>();
                viewModel.DateCreated = movie.DateCreated;
                viewModel.FileName = movie.FileName;
                viewModel.Location = movie.Location;
                viewModel.FileSize = movie.FileSize;
                viewModel.Remarks = movie.Remarks;
                viewModel.MovieId = movie.MovieId;
                viewModel.UpdateImage = true;
            }

            return View("Index", viewModel);
        }


        [HttpGet]
        public ActionResult Banner()
        {
            var banners = _bannerRepository.Find(x => !x.IsDeleted);
            return View(banners);
        }

        [HttpGet]
        public ActionResult AddBanner()
        {            
            return View("AddEditBanner", new BannerAddEditViewModel());
        }

        [HttpGet]
        public ActionResult EditBanner(int id)
        {
            var banner = _bannerRepository.GetById(id);
            if (banner != null)
            {
                var viewModel = banner.MapItem<BannerAddEditViewModel>();
                return View(viewModel);
            }

            //throw it to error page
            return View(banner);
        }

        private void UpdateImage(MovieViewModel viewModel)
        {
            var path = Server.MapPath("~/Resources/images");
            var fullPath = String.Format("{0}/{1}.jpg", path, viewModel.ImdbId);
            if (viewModel.MoviePoster != null && viewModel.UpdateImage)
            {
                viewModel.MoviePoster.SaveAs(fullPath);
                viewModel.Poster = fullPath;
            }
            else if (viewModel.UpdateImage && !String.IsNullOrEmpty(viewModel.Poster))
            {
                using (var client = new WebClient())
                {
                    client.DownloadFile(viewModel.Poster, fullPath);
                }
            }
        }        
    }
}
