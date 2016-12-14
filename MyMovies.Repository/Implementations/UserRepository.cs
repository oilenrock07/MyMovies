using System.Linq;
using MyMovies.Entities.Users;
using MyMovies.Infrastructure.Implementations;
using MyMovies.Infrastructure.Interfaces;
using MyMovies.Repository.Interfaces;

namespace MyMovies.Repository.Implementations
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(IDatabaseFactory databaseFactory)
            : base (databaseFactory)
        {
            DbSet = databaseFactory.GetContext().Users;
        }

        public virtual User GetUserByUserNameAndPassword(string username, string hashedPassword)
        {
            return Find(x => x.UserName == username && x.PasswordHash == hashedPassword).FirstOrDefault();
        }

        public virtual User GetById(string userId)
        {
            return Find(x => x.Id == userId).FirstOrDefault();
        }
    }
}
