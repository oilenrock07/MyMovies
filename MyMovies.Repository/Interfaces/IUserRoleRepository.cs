using System.Collections.Generic;
using MyMovies.Entities.Users;
using MyMovies.Infrastructure.Interfaces;

namespace MyMovies.Repository.Interfaces
{
    public interface IUserRoleRepository : IRepository<UserRole>
    {
        UserRole FindUserByRole(string userId, string roleId);
        void UpdateUserRole(string userId, IEnumerable<string> roleIds);
    }
}
