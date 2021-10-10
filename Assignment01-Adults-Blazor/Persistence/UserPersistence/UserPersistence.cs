using System;
using System.Linq;
using System.Net.Http.Headers;
using Assignment01_Adults_Blazor.Models;

namespace Assignment01_Adults_Blazor.Persistence.UserPersistence
{
    public class UserPersistence : IUserPersistence
    {
        private FileContext fileContext;

        public UserPersistence(FileContext fileContext)
        {
            this.fileContext = fileContext;
        }

        public void CreateUser(User user)
        {
            if (user == null)
                throw new ArgumentException("Fill in all the fields");

            try
            {
                User exists = fileContext.Users.FirstOrDefault(u => u.Username.Equals(user.Username));
                if (exists != null) throw new AggregateException("Username already exists");
                fileContext.Users.Add(user);
                fileContext.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public User ValidateUser(string username, string password)
        {
            if (string.IsNullOrEmpty(username)) throw new AggregateException("Invalid username or password");
            if (string.IsNullOrEmpty(password)) throw new AggregateException("Invalid username or password");

            User user = fileContext.Users.FirstOrDefault(u => u.Username.Equals(username));
            if (user == null) throw new ArgumentException("Invalid user");
            if (!user.Password.Equals(password)) throw new AggregateException("Invalid password!");

            return user;
        }
    }
}