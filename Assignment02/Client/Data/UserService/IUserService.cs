using System.Collections.Generic;
using System.Threading.Tasks;
using Model;

namespace Client.Data.UserService
{
    public interface IUserService
    {
        Task<IList<User>> GetAllUsersAsync();
        Task<User> CreateUserAsync(User user);
        Task<User> ValidateUser(User user);
    }
}