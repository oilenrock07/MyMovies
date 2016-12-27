using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using MyMovies.Common.BusinessLogic;
using MyMovies.Entities;
using MyMovies.Infrastructure.Implementations;
using MyMovies.Repository.Implementations;

namespace MyMovies.MovieInfoDownloader
{
    class Program
    {
        static void Main(string[] args)
        {
            var movieInfoDirectoryPath = ConfigurationManager.AppSettings["MovieInfoDownloadedPath"];
            var movieRemarksDirectoryPath = ConfigurationManager.AppSettings["MovieRemarksDownloadedPath"];
            var downloadMovieInfo = Convert.ToBoolean(ConfigurationManager.AppSettings["DownloadMovieInfo"]);
            var downloadMovieRemarks = Convert.ToBoolean(ConfigurationManager.AppSettings["DownloadMovieRemarks"]);
            
            var databaseFactory = new DatabaseFactory(new MovieContext());
            var movieXPathRepository = new MovieXPathRepository(databaseFactory, null); //should instantiate cache manager here
            var imdbScrapper = new ImdbScrapper(movieXPathRepository);

            if (!Directory.Exists(movieInfoDirectoryPath))
                Directory.CreateDirectory(movieInfoDirectoryPath);

            var dirMovieInfo = new DirectoryInfo(movieInfoDirectoryPath);
            var dirMovieRemarks = new DirectoryInfo(movieRemarksDirectoryPath);
            var downloadedMovieInfo = downloadMovieInfo ? dirMovieInfo.GetFiles().Select(fileInfo => fileInfo.Name).ToList() : new List<string>();
            var downloadedRemarks = downloadMovieRemarks ? dirMovieRemarks.GetFiles().Select(fileInfo => fileInfo.Name).ToList() : new List<string>();

            var movies = new List<string>(); //this will come from excel or csv
            foreach (var imdbId in movies)
            {
                if (downloadMovieInfo && !downloadedMovieInfo.Contains(imdbId))
                {
                    try
                    {
                        Console.WriteLine("*****Downloading {0}.  {1}/{2}", imdbId, downloadedMovieInfo.Count, movies.Count);
                        var document = imdbScrapper.GetMovieDocument(imdbId);
                        var fullPath = String.Format("{0}/{1}.txt", movieInfoDirectoryPath, imdbId);
                        using (var outputFile = new StreamWriter(fullPath))
                        {
                            outputFile.WriteLine(document);
                        }

                        downloadedMovieInfo.Add(imdbId);
                        Console.WriteLine("*****Movie {0} has been successfully downloaded", imdbId);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error downloading movie {0}\n{1}", imdbId, ex.Message);
                        Console.WriteLine("*****");
                    }
                }

                if (downloadMovieRemarks && !downloadedRemarks.Contains(imdbId))
                {
                    //download from rotten tomato
                    //save

                    downloadedRemarks.Add(imdbId);
                }
            }
        }
    }
}
