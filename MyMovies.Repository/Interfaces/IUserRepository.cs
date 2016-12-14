using MyMovies.Entities.Users;
using MyMovies.Infrastructure.Interfaces;

namespace MyMovies.Repository.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        User GetUserByUserNameAndPassword(string username, string hashedPassword);
        User GetById(string userId);
    }
}
