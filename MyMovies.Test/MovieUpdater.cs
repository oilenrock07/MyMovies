using System;
using System.Text;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyMovies.Common.BusinessLogic;
using MyMovies.Common.Extension;
using MyMovies.Entities;
using MyMovies.Infrastructure.Implementations;
using MyMovies.Infrastructure.Interfaces;
using MyMovies.Repository.Implementations;
using MyMovies.Repository.Interfaces;
using Omu.ValueInjecter;

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
            var toUpdateMovies = _movieRepository.GetAll().Where(x => String.IsNullOrEmpty(x.Poster) && String.IsNullOrEmpty(x.Writers)).OrderBy(x => x.Title).Take(529).ToList();
            foreach (var movie in toUpdateMovies)
            {
                string xmlDocument;
                var updatedMovie = scrapper.GetMovie(movie.ImdbId, out xmlDocument);
                UpdateMovie(movie, updatedMovie);
                var documentPath = String.Format("{0}/Documents", Environment.CurrentDirectory);
                var imagePath = String.Format("{0}/Images",Environment.CurrentDirectory);
                if (!Directory.Exists(imagePath))
                    Directory.CreateDirectory(imagePath);

                if (!Directory.Exists(documentPath))
                    Directory.CreateDirectory(documentPath);

                var uploadPath = String.Format("/Resources/images/{0}.jpg", movie.ImdbId);
                var documentFullPath = String.Format("{0}/{1}.txt", documentPath, movie.ImdbId);
                var fullPath = String.Format("{0}/{1}.jpg", imagePath, movie.ImdbId);

                if (!String.IsNullOrEmpty(updatedMovie.Poster))
                {
                    using (var client = new WebClient())
                    {
                        client.DownloadFile(updatedMovie.Poster, fullPath);
                        movie.Poster = uploadPath;
                    }
                }

                using (var file = new StreamWriter(documentFullPath))
                {
                    file.WriteLine(xmlDocument);
                }

                _unitOfWork.Commit();
            }
        }

        [TestMethod]
        public void UpdateMoviesFromFiles()
        {
            //get the imdbids from the files
            string documentPath = String.Format("{0}/Documents", Environment.CurrentDirectory);
            var dirInfo = new DirectoryInfo(documentPath);
            var scrapper = new ImdbScrapper(_xPathRepository);
            foreach (var fileInfo in dirInfo.GetFiles("*.txt"))
            {
                var updatedMovie = scrapper.LoadMovieFromFile(fileInfo.FullName);
                var movie = _movieRepository.GetByImdbId(fileInfo.Name);

                UpdateMovie(movie, updatedMovie);
                _unitOfWork.Commit();
            }
            //update the data
        }

        private void UpdateMovie(Movie movie, Movie updatedMovie)
        {
            var movieCopy = movie.MapItem<Movie>();

            _movieRepository.Update(movie);
            movie.InjectFrom(updatedMovie);
            movie.DateCreated = movieCopy.DateCreated;
            movie.FileName = !String.IsNullOrEmpty(movieCopy.FileName) ? movieCopy.FileName : "N/A";
            movie.Location = movieCopy.Location;
            movie.FileSize = !String.IsNullOrEmpty(movieCopy.FileSize) ? movieCopy.FileSize : "N/A";
            movie.Remarks = movieCopy.Remarks;
            movie.MovieId = movieCopy.MovieId;
        }
    }
}
