using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using IdentityModel;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
namespace HybridClientMvc
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();


            //nuget System.IdentityModel.Tokens.Jwt
            //nuget IdentityServer4

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            services.AddAuthentication(option =>
            {
                option.DefaultScheme = "Cookie.Client";
                option.DefaultChallengeScheme = "oidc";

            })
            .AddCookie("Cookie.Client", options => {

                options.AccessDeniedPath = "/Authorization/AccessDenied";
            })
           
            .AddOpenIdConnect("oidc", options => {
                options.SignInScheme = "Cookie.Client";
                options.Authority = "http://localhost:5000";
                options.RequireHttpsMetadata = false;
                options.ClientId = "Hybrid client";
                options.ClientSecret = "Hybrid Secret";
                options.SaveTokens = true;
                options.ResponseType = "code id_token";
                options.Scope.Clear();
                options.Scope.Add("api1");
                options.Scope.Add("openid");
                options.Scope.Add("profile");
                options.Scope.Add("roles");
                options.Scope.Add("locations");
                options.Scope.Add(OidcConstants.StandardScopes.OfflineAccess);

                //claims 角色要 = Mvc indentity role

                options.TokenValidationParameters = new TokenValidationParameters {

                    NameClaimType = JwtClaimTypes.Name,
                    RoleClaimType = JwtClaimTypes.Role,
                   
                };
            });

            //開始定義策回
            services.AddAuthorization(options => {

                options.AddPolicy("SmithinSomewhere", builder =>
                {
                    builder.RequireAuthenticatedUser();
                    builder.RequireClaim(JwtClaimTypes.FamilyName , "Smith");
                    builder.RequireClaim("location", "somewhere");
                });
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
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseAuthentication();  
            app.UseStaticFiles();

        
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
