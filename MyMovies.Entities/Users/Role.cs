using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyMovies.Entities.Users
{
    [Table("AspNetRoles")]
    public class Role 
    {
        [Key]
        public string Id { get; set; }

        [StringLength(250)]
        public string Name { get; set; }
    }
}
