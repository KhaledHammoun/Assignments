using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Model;

namespace Server.Persistence.UserPersistence
{
    public class UserPersistence : IUserPersistence
    {
        //private FileContext fileContext;

        public async Task<User> CreateUserAsync(User user)
        {
            if (user == null)
                throw new ArgumentException("Fill in all the fields");

            try
            {
                await using FamilyContext familyContext = new FamilyContext();
                User exists = await familyContext.Users.FirstOrDefaultAsync(u => u.Username.Equals(user.Username));
                if (exists != null) throw new AggregateException("Username already exists");
                await familyContext.Users.AddAsync(user);
                await familyContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return user;
        }

        public async Task<User> ValidateUserAsync(User userToValidate)
        {
            if (string.IsNullOrEmpty(userToValidate.Username)) throw new AggregateException("Invalid username or password");
            if (string.IsNullOrEmpty(userToValidate.Password)) throw new AggregateException("Invalid username or password");

            await using FamilyContext familyContext = new FamilyContext();
            User user = await familyContext.Users.FirstOrDefaultAsync(u => u.Username.Equals(userToValidate.Username));
            if (user == null) throw new ArgumentException("Invalid user");
            if (!user.Password.Equals(userToValidate.Password)) throw new AggregateException("Invalid password!");

            return user;
        }

        public async Task<IList<User>> GetAllUsersAsync()
        {
            await using FamilyContext familyContext = new FamilyContext();
            return familyContext.Users.ToList();
        }
    }
}