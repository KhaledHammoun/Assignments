using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Client.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Client.Data;
using Client.Data.AdultService;
using Client.Data.UserService;
using Microsoft.AspNetCore.Components.Authorization;

namespace Client
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
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddSingleton<IAdultService, AdultService>();
            services.AddSingleton<IUserService, UserService>();
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