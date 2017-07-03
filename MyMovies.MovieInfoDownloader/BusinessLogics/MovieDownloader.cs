using MyMovies.Common.BusinessLogic;
using MyMovies.Common.Extension;
using MyMovies.Entities;
using MyMovies.Infrastructure.Implementations;
using MyMovies.MovieInfoDownloader.Interfaces;
using MyMovies.Repository.Implementations;
using MyMovies.Repository.Interfaces;
using Omu.ValueInjecter;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;

namespace MyMovies.MovieInfoDownloader.BusinessLogics
{
    public class MovieDownloader : MovieBase, IMovieProcessHandler
    {
        private static IMovieRepository _movieRepository;

        public void Process()
        {
            var movieInfoDirectoryPath = ConfigurationManager.AppSettings["MovieInfoDownloadedPath"];
            var movieRemarksDirectoryPath = ConfigurationManager.AppSettings["MovieRemarksDownloadedPath"];
            var downloadMovieInfo = Convert.ToBoolean(ConfigurationManager.AppSettings["DownloadMovieInfo"]);
            var downloadMovieRemarks = Convert.ToBoolean(ConfigurationManager.AppSettings["DownloadMovieRemarks"]);

            if (!Directory.Exists(movieInfoDirectoryPath))
                Directory.CreateDirectory(movieInfoDirectoryPath);

            var dirMovieInfo = new DirectoryInfo(movieInfoDirectoryPath);
            var dirMovieRemarks = new DirectoryInfo(movieRemarksDirectoryPath);
            var downloadedMovieInfo = downloadMovieInfo ? dirMovieInfo.GetFiles().Select(fileInfo => fileInfo.Name).ToList() : new List<string>();
            var downloadedRemarks = downloadMovieRemarks ? dirMovieRemarks.GetFiles().Select(fileInfo => fileInfo.Name).ToList() : new List<string>();

            var scrapper = new ImdbScrapper(_movieXPathRepository);
            var movies = _movieRepository.GetAll().Where(x => String.IsNullOrEmpty(x.Poster)).OrderBy(x => x.Title).Take(250).ToList();

            foreach (var movie in movies)
            {
                if (downloadMovieInfo && !downloadedMovieInfo.Contains(movie.ImdbId))
                {
                    try
                    {
                        Console.WriteLine("*****Downloading {0}.  {1}/{2}", movie.ImdbId, downloadedMovieInfo.Count, movies.Count);
                        var document = _imdbScrapper.GetMovieDocument(movie.ImdbId);
                        var fullPath = String.Format("{0}/{1}.txt", movieInfoDirectoryPath, movie.ImdbId);
                        using (var outputFile = new StreamWriter(fullPath))
                        {
                            outputFile.WriteLine(document);
                        }

                        downloadedMovieInfo.Add(movie.ImdbId);
                        Console.WriteLine("*****Movie {0} has been successfully downloaded", movie.ImdbId);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error downloading movie {0}\n{1}", movie.ImdbId, ex.Message);
                        Console.WriteLine("*****");
                    }
                }

                if (downloadMovieRemarks && !downloadedRemarks.Contains(movie.ImdbId))
                {
                    //download from rotten tomato
                    //save

                    downloadedRemarks.Add(movie.ImdbId);
                }
            }
        }

        private static void UpdateMovie(Movie movie, Movie updatedMovie)
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
