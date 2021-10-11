using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using Assignment01_Adults_Blazor.Models;
using Assignment01_Adults_Blazor.Persistence.UserPersistence;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;


namespace Assignment01_Adults_Blazor.Authentication
{
    public class CurrentAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly IJSRuntime jsRuntime;
        private readonly IUserPersistence userPersistence;

        private User cachedUser;

        public CurrentAuthenticationStateProvider(IJSRuntime jsRuntime, IUserPersistence userPersistence)
        {
            this.jsRuntime = jsRuntime;
            this.userPersistence = userPersistence;
        }
        
        
        
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var identity = new ClaimsIdentity();
            if (cachedUser == null)
            {
                string userAsJson = await jsRuntime.InvokeAsync<string>("sessionStorage.getItem", "currentUser");
                if (!string.IsNullOrEmpty(userAsJson))
                {
                    User user = JsonSerializer.Deserialize<User>(userAsJson);
                    ValidateUser(user.Username, user.Password);
                }
            }
            else
            {
                identity = SetupClaimsForUser(cachedUser);
            }

            ClaimsPrincipal cachedClaimsPrincipal = new ClaimsPrincipal(identity);
            return await Task.FromResult(new AuthenticationState(cachedClaimsPrincipal));
        }

        private ClaimsIdentity SetupClaimsForUser(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim("Id", user.Id.ToString()),
                new Claim("FirstName", user.FirstName),
                new Claim("LastName", user.LastName),
                new Claim("HairColor", user.HairColor),
                new Claim("EyeColor", user.EyeColor),
                new Claim("Age", user.Age.ToString()),
                new Claim("Weight", user.Weight.ToString()),
                new Claim("Sex", user.Sex),
                new Claim("Level", user.SecurityLevel.ToString())
            };
            ClaimsIdentity identity = new ClaimsIdentity(claims, "apiauth_type");
            return identity;
        }

        public void ValidateUser(string username, string password)
        {
            if (string.IsNullOrEmpty(username)) throw new Exception("Enter username");
            if (string.IsNullOrEmpty(password)) throw new Exception("Enter password");

            var identity = new ClaimsIdentity();
            try
            {
                var user = userPersistence.ValidateUser(username, password);
                identity = SetupClaimsForUser(user);
                string userAsJson = JsonSerializer.Serialize(user);
                jsRuntime.InvokeVoidAsync("sessionStorage.setItem", "currentUser", userAsJson);
                cachedUser = user;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(new ClaimsPrincipal(identity))));
        }

        public void Logout()
        {
            cachedUser = null;
            var user = new ClaimsPrincipal(new ClaimsIdentity());
            jsRuntime.InvokeVoidAsync("sessionStorage.setItem", "currentUser", user);
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }
    }
}