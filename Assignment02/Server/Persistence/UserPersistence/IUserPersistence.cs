using System.Collections.Generic;
using System.Threading.Tasks;
using Model;

namespace Server.Persistence.UserPersistence
{
    public interface IUserPersistence
    {
        Task<User> CreateUserAsync(User user);
        Task<User> ValidateUserAsync(User user);

        Task<IList<User>> GetAllUsersAsync();
    }
}