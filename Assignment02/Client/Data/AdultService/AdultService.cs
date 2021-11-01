using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Model;

namespace Client.Data.AdultService
{
    public class AdultService : IAdultService
    {
        private readonly string uri = "https://localhost:5003/Adults";
        private readonly HttpClient client;

        public AdultService()
        {
            client = new HttpClient();
        }
        
        public async Task<IList<Adult>> GetAllAdultsAsync()
        {
            var response = await client.GetAsync(uri);
            if (!response.IsSuccessStatusCode)
                throw new Exception($"Code: {response.StatusCode}, {response.ReasonPhrase}");
            var message = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<IList<Adult>>(message, new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
            return result;
        }

        public async Task<Adult> GetAdultByIdAsync(int id)
        {
            var response = await client.GetAsync(uri + $"/{id}");
            var message = await response.Content.ReadAsStringAsync();
            var toReturn = JsonSerializer.Deserialize<Adult>(message, new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
            return toReturn;
        }

        public async Task<Adult> AddAdultAsync(Adult adult)
        {
            var adultAsJson = JsonSerializer.Serialize(adult);
            var content = new StringContent(adultAsJson, Encoding.UTF8, "application/json");
            var response = client.PostAsync(uri, content);
            var message = await response.Result.Content.ReadAsStringAsync();
            var toReturn = JsonSerializer.Deserialize<Adult>(message, new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
            return toReturn;
        }

        public async Task DeleteAdultAsync(int id)
        {
            var response = await client.DeleteAsync(uri + $"/{id}");
            if (!response.IsSuccessStatusCode)
                throw new Exception($"Code: {response.StatusCode}, {response.ReasonPhrase}");
        }

        public async Task<Adult> EditAdultAsync(Adult adult)
        {
            var adultAsJson = JsonSerializer.Serialize(adult);
            var content = new StringContent(adultAsJson, Encoding.UTF8, "application/json");
            var response = await client.PutAsync(uri + $"/{adult.Id}", content);
            var message = await response.Content.ReadAsStringAsync();
            var toReturn = JsonSerializer.Deserialize<Adult>(message, new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
            return toReturn;
        }
    }
}