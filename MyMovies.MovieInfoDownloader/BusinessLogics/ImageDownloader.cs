using MyMovies.MovieInfoDownloader.Interfaces;
using System;
using System.CodeDom;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Collections.Generic;

namespace MyMovies.MovieInfoDownloader.BusinessLogics
{
    public class ImageDownloader : MovieBase, IMovieProcessHandler
    {
        public void Process()
        {
            var imagePath = ConfigurationManager.AppSettings["ImagesPath"];
            var documentPath = ConfigurationManager.AppSettings["MovieInfoDownloadedPath"];
            var imagesToUpdateSize = Convert.ToInt32(ConfigurationManager.AppSettings["ImagesToUpdateSize"]);
            var imageDirInfo = new DirectoryInfo(imagePath);

            var images = imageDirInfo.GetFiles("*.jpg", SearchOption.TopDirectoryOnly);
            if (images.Any())
            {
                images = images.Where(x => (x.Length / 1024) < imagesToUpdateSize).ToArray();
                var totalImages = images.Count();
                var ctr = 1;

                var actions = new List<Action>();
                foreach (var item in images)
                {
                    var fileName = Path.GetFileNameWithoutExtension(item.Name);
                    try
                    {                        
                        Console.WriteLine("*****Downloading movie: {0} {1}/{2}", fileName, ctr, totalImages);

                        var documentName = String.Format("{0}.txt", fileName);
                        var movieFromFile = _imdbScrapper.LoadMovieFromFile(Path.Combine(documentPath, documentName));
                        var fullPath = Path.Combine(imagePath, item.Name);

                        var uploadPath = String.Format("/Resources/images/{0}.jpg", fileName);
                        if (movieFromFile != null)
                        {
                            using (var client = new WebClient())
                            {
                                client.DownloadFile(movieFromFile.Poster, fullPath);
                            }

                            //update the record in db
                            var movie = _movieRepository.GetByImdbId(fileName);
                            movie.HdPosterLink = movie.HdPosterLink;

                            _unitOfWork.Commit();
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error downloading image for {0}", fileName);
                        continue;
                    }


                    ctr++;
                }
            }
        }
    }
}
