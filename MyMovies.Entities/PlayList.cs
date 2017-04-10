using System;
using System.ComponentModel.DataAnnotations;

namespace MyMovies.Entities
{
    public class PlayList
    {
        [Key]
        public int PlayListId { get; set; }

        public int UserId { get; set; }

        [StringLength(250)]
        public string PlayListName { get; set; }

        public string Description { get; set; }

        public DateTime DateCreated { get; set; }
    }
}
