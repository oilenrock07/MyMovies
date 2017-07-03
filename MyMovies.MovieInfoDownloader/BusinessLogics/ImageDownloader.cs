using MyMovies.MovieInfoDownloader.Interfaces;
using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;

namespace MyMovies.MovieInfoDownloader.BusinessLogics
{
    public class ImageDownloader : MovieBase, IMovieProcessHandler
    {
        public void Process()
        {                        
            var imagePath = ConfigurationManager.AppSettings["ImagesPath"];
            var documentPath = ConfigurationManager.AppSettings["MovieInfoDownloadedPath"];
            var imagesToUpdateSize = Convert.ToDouble(ConfigurationManager.AppSettings["ImagesToUpdateSize"]);
            var imageDirInfo = new DirectoryInfo(imagePath);

            var images = imageDirInfo.GetFiles("*.jpg", SearchOption.TopDirectoryOnly);
            if (images != null)
            {                
                images = images.Where(x => (x.Length / 1024) < 50).ToArray();
                var totalImages = images.Count();
                var ctr = 1;

                foreach(var item in images)
                {
                    var fileName = Path.GetFileNameWithoutExtension(item.Name);
                    Console.WriteLine("*****Downloading movie: {0} {1}/{2}", fileName, ctr, totalImages);

                    var documentName = String.Format("{0}.txt", fileName);
                    var movieFromFile = _imdbScrapper.LoadMovieFromFile(Path.Combine(documentPath, documentName));
                    var fullPath = Path.Combine(imagePath, item.Name);

                    if (movieFromFile != null)
                    {
                        //download the updated image
                        using (var client = new WebClient())
                        {
                            client.DownloadFile(movieFromFile.Poster, fullPath);
                        }

                        //update the record in db
                        var movie = _movieRepository.GetByImdbId(movieFromFile.ImdbId);
                        movie.OriginalPoster = movieFromFile.OriginalPoster;

                        _unitOfWork.Commit();
                    }

                    ctr++;
                }
            }
        }
    }
}
