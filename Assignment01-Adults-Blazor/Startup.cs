using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Assignment01_Adults_Blazor.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Assignment01_Adults_Blazor.Data;
using Assignment01_Adults_Blazor.Persistence;
using Assignment01_Adults_Blazor.Persistence.AdultPersistance;
using Assignment01_Adults_Blazor.Persistence.AdultPersistence;
using Assignment01_Adults_Blazor.Persistence.UserPersistence;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Assignment01_Adults_Blazor
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        /*
         * Authentication levels:
         *      Level 1 - Can see all adults without editing and deleting. Can filter.
         *      Level 2 - Can add adult and edit. Cant delete.
         *      Level 3 - Can delete adults.
         * TODO Level 4 - Can see statistics    
         */
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddSingleton<WeatherForecastService>();
            services.AddSingleton<FileContext>();
            services.AddSingleton<IAdultPersistence, AdultPersistence>();
            services.AddSingleton<IUserPersistence, UserPersistence>();
            services.AddScoped<AuthenticationStateProvider, CurrentAuthenticationStateProvider>();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Level1", policy => policy.RequireAuthenticatedUser().RequireAssertion(context =>
                {
                    Claim claim = context.User.FindFirst(claim1 => claim1.Type.Equals("Level"));
                    if (claim == null) return false;
                    return int.Parse(claim.Value) >= 1;
                }));

                options.AddPolicy("Level2", policy => policy.RequireAuthenticatedUser().RequireAssertion(context =>
                {
                    Claim securityLevel = context.User.FindFirst(claim => claim.Type.Equals("Level"));
                    if (securityLevel == null) return false;
                    return int.Parse(securityLevel.Value) >= 2;
                }));

                options.AddPolicy("Level3", policy => policy.RequireAuthenticatedUser().RequireAssertion(context =>
                {
                    Claim securityLevel = context.User.FindFirst(claim => claim.Type.Equals("Level"));
                    if (securityLevel == null) return false;
                    return int.Parse(securityLevel.Value) >= 3;
                }));
    
                options.AddPolicy("Level4", policy => policy.RequireAuthenticatedUser().RequireAssertion(context =>
                {
                    Claim securityLevel = context.User.FindFirst(claim => claim.Type.Equals("Level"));
                    if (securityLevel == null) return false;
                    return int.Parse(securityLevel.Value) >= 4;
                }));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}