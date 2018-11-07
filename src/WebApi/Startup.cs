using System;
using System.Text;
using System.Threading.Tasks;
using Core.Abstractions;
using Core.Services;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Mvc;


namespace WebApi
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
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    RequireExpirationTime = true,
                    ClockSkew = TimeSpan.FromMinutes(0),
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["Auth:ValidIssuer"],
                    ValidAudience = Configuration["Auth:ValidAudience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Auth:SecurityKey"]))
                };
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddSingleton(Configuration);

            services.AddScoped<IIdentityService, IdentityService>();
            services.AddScoped<ITokenService, TokenService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseCors(
                options => options
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
            );

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseAuthentication();
            
            app.UseMiddleware<RandomFailureMiddleware>(); 

            app.UseHttpsRedirection();
            app.UseMvc();
        }

        private class RandomFailureMiddleware
        {
            private readonly RequestDelegate _next;
            private readonly Random _rand;

            public RandomFailureMiddleware(RequestDelegate next)
            {
                _next = next;
                _rand = new Random();
            }

            public Task Invoke(HttpContext httpContext)
            {
                if (_rand.NextDouble() >= 0.5)
                {
                    //throw new Exception("Computer says no.");
                }           

                return _next(httpContext);
            }
        }
    }   
}