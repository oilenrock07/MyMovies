﻿using System;
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
            else
            {
                count = _movieRepository.GetAll().Count();
                movies = _movieRepository.GetAll();
            }

            var viewModel = new MoviePaginationViewModel();
            viewModel.Pagination = pagination.GetPaginationModel(Request, count);
            viewModel.Movies = pagination.TakePaginationModel(movies.OrderBy(x => x.Title).ToList(), viewModel.Pagination).MapCollection<MovieViewModel>();
            
            return View(viewModel);
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
    }
}
