using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyMovies.Entities.Users
{
    [Table("AspNetUserRoles")]
    public class UserRole
    {

        [Key]
        [Column(Order = 0)]
        [StringLength(250)]
        public string UserId { get; set; }

        [Column(Order = 1)]
        [StringLength(250)]
        public string RoleId { get; set; }
    }
}
