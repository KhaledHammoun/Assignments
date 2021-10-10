using Assignment01_Adults_Blazor.Models;

namespace Assignment01_Adults_Blazor.Persistence.UserPersistence
{
    public interface IUserPersistence
    {
        void CreateUser(User user);
        User ValidateUser(string username, string password);
    }
}