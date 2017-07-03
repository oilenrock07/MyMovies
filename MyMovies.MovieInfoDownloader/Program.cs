using MyMovies.MovieInfoDownloader.BusinessLogics;

namespace MyMovies.MovieInfoDownloader
{
    public class Program
    {
        private static void Main(string[] args)
        {
            var imageDownloader = new ImageDownloader();
            imageDownloader.Process();
        }
    }
}
