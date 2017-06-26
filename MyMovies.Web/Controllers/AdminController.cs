using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;
using MyMovies.Common.BusinessLogic;
using MyMovies.Common.Extension;
using MyMovies.Entities;
using MyMovies.Infrastructure.Interfaces;
using MyMovies.Repository.Interfaces;
using MyMovies.Web.ViewModels;
using Omu.ValueInjecter;
using System.Net;
using Microsoft.Ajax.Utilities;
using MyMovies.Common.Helpers;

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

            ViewBag.Menu = "AdminMovie";
            return View(viewModel);
        }

        public ActionResult Search(string key)
        {
            Movie movie;
            key = key.Trim();
            if (key.IsIMDB() || key.IsIMDBUrl())
            {
                if (key.IsIMDBUrl())
                    key = ImdbHelper.GetImdbId(key);

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
                if (movie == null)
                {
                    movie = new Movie();
                    TempData["NoSearchResult"] = true;
                }
            }

            ViewBag.Search = key;
            var viewModel = movie.MapItem<MovieViewModel>();
            return View("Index", viewModel);
        }

        [HttpPost]
        [ValidateInput(false)]
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

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult GetHdImage(string url, string imdbId)
        {
            var scrapper = new ImdbScrapper(_movieXPathRepository);
            var hdUrl = scrapper.GetHdImage(url, imdbId);
            return Json(hdUrl);
        }

        [HttpGet]
        public ActionResult SearchImdbTitle(string key)
        {
            var scrapper = new ImdbScrapper(_movieXPathRepository);
            var movies = scrapper.SearchMovieByTitle(key);

            return View(movies);
        }

        [HttpGet]
        public ActionResult Banner()
        {
            ViewBag.Menu = "AdminBanner";
            var banners = _bannerRepository.Find(x => !x.IsDeleted);
            return View(banners);
        }

        [HttpGet]
        public ActionResult AddBanner()
        {
            ViewBag.Menu = "AdminBanner";
            return View("AddEditBanner", new BannerAddEditViewModel() { TextColor = "White"});
        }

        [HttpGet]
        public ActionResult EditBanner(int id)
        {
            ViewBag.Menu = "AdminBanner";
            var banner = _bannerRepository.GetById(id);
            if (banner != null)
            {
                var viewModel = banner.MapItem<BannerAddEditViewModel>();
                return View("AddEditBanner", viewModel);
            }

            //throw it to error page
            return View(banner);
        }

        [HttpGet]
        public ActionResult DeleteBanner(int id)
        {
            var banner = _bannerRepository.GetById(id);
            if (banner != null)
            {
                banner.IsDeleted = true;           
                _unitOfWork.Commit();
                return RedirectToAction("Banner");
            }

            //throw it to error page
            return View(banner);
        }

        [HttpPost]
        public ActionResult AddEditBanner(BannerAddEditViewModel viewModel)
        {
            ViewBag.Menu = "AdminBanner";
            if (viewModel.MoviePoster == null && String.IsNullOrEmpty(viewModel.Poster))
            {
                ViewBag.Error = "Please select a background image";
                return View(viewModel);
            }

            var guid = Guid.NewGuid();
            var path = Server.MapPath("~/Resources/banners");
            var uploadPath = String.Format("/Resources/banners/{0}.jpg", guid);
            var fullPath = String.Format("{0}/{1}.jpg", path, guid);

            if (viewModel.BannerId > 0)
            {
                var banner = _bannerRepository.GetById(viewModel.BannerId);
                banner.Poster = uploadPath;
                banner.InjectFrom(viewModel);
            }
            else
            {
                var newBanner = viewModel.MapItem<Banner>();
                newBanner.Poster = uploadPath;
                _bannerRepository.Add(newBanner);
            }
            
            if (viewModel.MoviePoster != null)
                viewModel.MoviePoster.SaveAs(fullPath);

            _unitOfWork.Commit();

            return RedirectToAction("Banner");
        }

        private void UpdateImage(MovieViewModel viewModel)
        {
            var path = Server.MapPath("~/Resources/images");
            var uploadPath = String.Format("/Resources/images/{0}.jpg", viewModel.ImdbId);
            var fullPath = String.Format("{0}/{1}.jpg", path, viewModel.ImdbId);
            if (viewModel.MoviePoster != null && viewModel.UpdateImage)
            {
                viewModel.MoviePoster.SaveAs(fullPath);
                viewModel.Poster = uploadPath;
            }
            else if ((viewModel.UpdateImage || viewModel.MovieId == 0) && !String.IsNullOrEmpty(viewModel.Poster))
            {
                using (var client = new WebClient())
                {
                    client.DownloadFile(viewModel.Poster, fullPath);
                    viewModel.Poster = uploadPath;
                }
            }
        }        
    }
}
