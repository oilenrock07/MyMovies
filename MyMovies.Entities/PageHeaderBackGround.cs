using System.ComponentModel.DataAnnotations;

namespace MyMovies.Entities
{
    public class PageHeaderBackGround
    {
        [Key]
        public int PageHeaderBackGroundId { get; set; }

        public byte Type { get; set; }
        public string Image { get; set; }
        public string Colour { get; set; }
        public string Description { get; set; }
    }
}
