using MyMovies.MovieInfoDownloader.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.IO;

namespace MyMovies.MovieInfoDownloader.BusinessLogics
{
    public class HDPosterLinkUpdater : MovieBase, IMovieProcessHandler
    {
        public void Process()
        {
            var moviesToUpdate = _movieRepository.Find(x => x.HdPosterLink.Contains("/Resources/")).ToList();
            var documentPath = ConfigurationManager.AppSettings["MovieInfoDownloadedPath"];
            if (moviesToUpdate.Any())
            {
                var totalMoviesToUpdate = moviesToUpdate.Count();
                var ctr = 1;

                foreach (var item in moviesToUpdate)
                {
                    try
                    {
                        Console.WriteLine("*****Downloading movie: {0} {1}/{2}", item.MovieId, ctr, totalMoviesToUpdate);
                        var documentName = String.Format("{0}.txt", item.ImdbId);
                        var movieFromFile = _imdbScrapper.LoadMovieFromFile(Path.Combine(documentPath, documentName));

                        if (movieFromFile != null)
                        {
                            item.HdPosterLink = movieFromFile.HdPosterLink;
                            _unitOfWork.Commit();
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error downloading image for {0}", item.MovieId);
                        continue;
                    }


                    ctr++;
                }
            }
        }
    }
}
