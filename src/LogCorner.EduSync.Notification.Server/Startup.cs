using LogCorner.EduSync.SignalR.Server.Hubs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Threading.Tasks;

namespace LogCorner.EduSync.Notification.Server
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
            services.AddCors(options =>
            {
                var allowedOrigins = Configuration["allowedOrigins"]?.Split(",");
                options.AddPolicy("corsPolicy",
                    builder =>
                        builder.AllowAnyHeader()
                            .AllowAnyMethod()
                            .WithOrigins(allowedOrigins)
                            .AllowCredentials()
                    );
            });

            services
               .AddAuthentication()
               .AddJwtBearer("AAD", options =>
               {
                   options.Authority = $"{Configuration["AzureAd:Instance"]}/{Configuration["AzureAd:TenantId"]}/v2.0";
                   options.TokenValidationParameters = new TokenValidationParameters
                   {
                       ValidateIssuer = true,
                       ValidIssuer = $"{Configuration["AzureAd:Instance"]}/{Configuration["AzureAd:TenantId"]}/v2.0",
                       ValidateAudience = true,
                       ValidAudience = Configuration["AzureAd:ClientId"],
                       ValidateLifetime = true,
                       NameClaimType = "name"
                   };
               })

               .AddJwtBearer("B2C", options =>
               {
                   options.Authority = $"{Configuration["AzureAdB2C:Instance"]}/tfp/{Configuration["AzureAdB2C:TenantId"]}/{Configuration["AzureAdB2C:SignUpSignInPolicyId"]}/v2.0/";
                   options.TokenValidationParameters = new TokenValidationParameters
                   {
                       ValidateIssuer = true,
                       ValidIssuer = $"{Configuration["AzureAdB2C:Instance"]}/{Configuration["AzureAdB2C:TenantId"]}/v2.0/",
                       ValidateAudience = true,
                       ValidAudience = Configuration["AzureAdB2C:ClientId"],
                       ValidateLifetime = true,
                       NameClaimType = "name"
                   };
                   options.Events = new JwtBearerEvents
                   {
                       OnMessageReceived = context =>
                       {
                           var accessToken = context.Request.Query["access_token"];

                           // If the request is for our hub...
                           var path = context.HttpContext.Request.Path;
                           if (!string.IsNullOrEmpty(accessToken) &&
                               (path.StartsWithSegments("/logcornerhub")))
                           {
                               // Read the token out of the query string
                               context.Token = accessToken;
                           }
                           return Task.CompletedTask;
                       }
                   };
               });
            services
                .AddAuthorization(options =>
                {
                    options.DefaultPolicy = new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                        .AddAuthenticationSchemes("AAD", "B2C")
                        .Build();

                    options.AddPolicy("AADAdmins", new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                        .AddAuthenticationSchemes("AAD")

                        .Build());
                });

            services.AddSignalR(log =>
            {
                log.EnableDetailedErrors = true;
            });
            services.AddControllers();
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
                app.UseHsts();
                app.UseHttpsRedirection( );
            }
            app.UseCors("corsPolicy");
           

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<LogCornerHub<object>>("/logcornerhub").RequireAuthorization();
            });
            string pathBase = Configuration["pathBase"];
            if (!string.IsNullOrWhiteSpace(pathBase))
            {
                app.UsePathBase(new PathString(pathBase));
            }
        }
    }
}