using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Model;

namespace Client.Data.UserService
{
    public class UserService : IUserService
    {
        private readonly HttpClient client;
        private readonly string uri = "https://localhost:5003/Users";

        public UserService()
        {
            client = new HttpClient();
        }

        public Task<IList<User>> GetAllUsersAsync()
        {
            //Not needed for now. Only for test purposes 
            throw new System.NotImplementedException();
        }

        public async Task<User> CreateUserAsync(User user)
        {
            var userAsJson = JsonSerializer.Serialize(user);
            var content = new StringContent(userAsJson, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(uri + "/create", content);
            var message = await response.Content.ReadAsStringAsync();
            var toReturn = JsonSerializer.Deserialize<User>(message, new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
            return toReturn;
        }

        public async Task<User> ValidateUser(User user)
        {
            var userToSend = JsonSerializer.Serialize(user);
            var content = new StringContent(userToSend, Encoding.UTF8, "application/json");
            var message = await client.PostAsync(uri + "/validate", content);
            var readAsStringAsync = message.Content.ReadAsStringAsync();
            var toReturn = JsonSerializer.Deserialize<User>(readAsStringAsync.Result, new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
            return toReturn;
        }
    }
}