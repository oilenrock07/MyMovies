using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyMovies.Entities;
using MyMovies.Entities.Enums;

namespace MyMovies.Web.ViewModels
{
    public class BannerAddEditViewModel : Banner
    {
        public HttpPostedFileBase MoviePoster { get; set; }
        private IEnumerable<SelectListItem> _bannerTypes;
        public IEnumerable<SelectListItem> BannerTypes
        {
            get
            {
                if (_bannerTypes == null) GetBannerTypes();
                return _bannerTypes;
            }
        }

        private void GetBannerTypes()
        {
            var list = (from BannerType bannerType in Enum.GetValues(typeof (BannerType))
                select new SelectListItem
                {
                    Text = bannerType.ToString(),
                    Value = ((int) bannerType).ToString()
                }).ToList();

            _bannerTypes = list;
        }

        public string HeaderBg
        {
            get
            {
                return String.IsNullOrEmpty(Poster) ? "'/Images/Banners/Default.jpg'" : String.Format("'{0}'", Poster);
            }
        }
    }
}