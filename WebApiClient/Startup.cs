using System;
using Core.Abstractions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using Polly;

namespace WebApiClient
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

            services.AddRefitClient<IWebApi>()
                .ConfigureHttpClient(c => c.BaseAddress = new Uri(Configuration["WebApi"]))
                .AddTransientHttpErrorPolicy(p => p.RetryAsync(6));
            // Add additional IHttpClientBuilder chained methods as required here:
            //.AddHttpMessageHandler<MyHandler>();
            //.SetHandlerLifetime(TimeSpan.FromMinutes(2));
            // more... https://github.com/App-vNext/Polly.Extensions.Http

            services.AddSingleton(Configuration);
            
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}