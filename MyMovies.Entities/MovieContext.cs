using System.Data.Entity;
using MyMovies.Entities.Users;

namespace MyMovies.Entities
{
    public class MovieContext : DbContext
    {
        public MovieContext():
            base("MovieConnectionString")
        {                
        }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<MovieXPath> MovieXPaths { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Banner> Banners { get; set; }

        //Users
        public virtual IDbSet<Role> Roles { get; set; }
        public virtual IDbSet<User> Users { get; set; }
        //public virtual IDbSet<UserClaim> UserClaims { get; set; }
        //public virtual IDbSet<UserLogin> UserLogIns { get; set; }
        public virtual IDbSet<UserRole> UserRoles { get; set; }
    }
}
