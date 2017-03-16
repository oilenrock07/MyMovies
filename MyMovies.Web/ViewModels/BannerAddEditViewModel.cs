using System.Web;
using MyMovies.Entities;

namespace MyMovies.Web.ViewModels
{
    public class BannerAddEditViewModel : Banner
    {
        public HttpPostedFileBase MoviePoster { get; set; }
    }
}