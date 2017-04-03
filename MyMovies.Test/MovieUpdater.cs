using System;
using System.Text;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyMovies.Common.BusinessLogic;
using MyMovies.Entities;
using MyMovies.Infrastructure.Implementations;
using MyMovies.Infrastructure.Interfaces;
using MyMovies.Repository.Implementations;
using MyMovies.Repository.Interfaces;

namespace MyMovies.Test
{
    /// <summary>
    /// This is not a unit test. This is standalone application to get the latest movie details instead of creating a new Console Project.
    /// </summary>
    [TestClass]
    public class MovieUpdater
    {
        private readonly IMovieXPathRepository _xPathRepository;
        private readonly IMovieRepository _movieRepository;
        private readonly IUnitOfWork _unitOfWork;

        public MovieUpdater()
        {
            IDatabaseFactory databaseFactory = new DatabaseFactory(new MovieContext());
            _xPathRepository = new MovieXPathRepository(databaseFactory, null);
            _movieRepository = new MovieRepository(databaseFactory);
            _unitOfWork = new UnitOfWork(databaseFactory);
        }

        [TestMethod]
        public void UpdateNotUpdatedMovies()
        {
            var scrapper = new ImdbScrapper(_xPathRepository);
            var toUpdateMovies = _movieRepository.GetAll().Where(x => String.IsNullOrEmpty(x.Poster)).ToList();
            foreach (var movie in toUpdateMovies)
            {
                string xmlDocument;
                var updatedMovie = scrapper.GetMovie(movie.ImdbId, out xmlDocument);
                updatedMovie.DateCreated = movie.DateCreated;
                updatedMovie.FileName = movie.FileName;
                updatedMovie.Location = movie.Location;
                updatedMovie.FileSize = movie.FileSize;
                updatedMovie.Remarks = movie.Remarks;
                updatedMovie.MovieId = movie.MovieId;

                var imagePath = String.Format("{0}/Images",Environment.CurrentDirectory);
                var documentPath = String.Format("{0}/Documents", Environment.CurrentDirectory);
                if (!Directory.Exists(imagePath))
                    Directory.CreateDirectory(imagePath);

                if (!Directory.Exists(documentPath))
                    Directory.CreateDirectory(documentPath);

                var uploadPath = String.Format("/Resources/images/{0}.jpg", movie.ImdbId);
                var documentFullPath = String.Format("{0}/{1}.txt", documentPath, movie.ImdbId);
                var fullPath = String.Format("{0}/{1}.jpg", imagePath, movie.ImdbId);
                using (var client = new WebClient())
                {
                    client.DownloadFile(updatedMovie.Poster, fullPath);
                    updatedMovie.Poster = uploadPath;
                }

                using (var file = new StreamWriter(documentFullPath))
                {
                    file.WriteLine(xmlDocument);
                }

                _unitOfWork.Commit();
            }
        }
    }
}
