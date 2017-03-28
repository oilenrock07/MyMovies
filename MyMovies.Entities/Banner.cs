using System.ComponentModel.DataAnnotations;
using MyMovies.Entities.Enums;

namespace MyMovies.Entities
{
    public class Banner
    {
        [Key]
        public int BannerId { get; set; }

        public string Poster { get; set; }

        public string TextColor { get; set; }

        public string Identifier { get; set; }

        public bool IsDeleted { get; set; }

        public BannerType Type { get; set; }
    }
}
