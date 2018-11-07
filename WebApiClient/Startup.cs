using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Core.Abstractions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
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
        public static IAuthService AuthService;

        private static string Token { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public async void ConfigureServices(IServiceCollection services)
        {
            //RefreshAuthorization();
            
            Token = "Bearer IntentionalBadToken";
            
            var retry = Policy<HttpResponseMessage> 
                .HandleResult(r => r.StatusCode == HttpStatusCode.Unauthorized)
                .RetryAsync(3, async (ex, retryCount) => 
                {
                    // access token expired or identity api ep was unavailable
                    if (retryCount < 3)
                    {
                        //Thread.Sleep(500);
                        await RefreshAuthorization();
                    }
                    // refresh token expired
                    if (retryCount == 3)
                    {
                        throw new Exception();
                    }
                });

            services.AddRefitClient<IWebApi>()
                .ConfigureHttpClient(async c =>
                {
                    c.BaseAddress = new Uri(Configuration["WebApi"]);
                    c.DefaultRequestHeaders.Add("Authorization", Token);
                })
                .AddTransientHttpErrorPolicy(p => p.RetryAsync(6))
                .AddPolicyHandler(retry)      
                // Add additional IHttpClientBuilder chained methods as required here:
                
                .AddHttpMessageHandler<AutorizationHandler>();//.SetHandlerLifetime(TimeSpan.FromSeconds(49));;
                // more... https://github.com/App-vNext/Polly.Extensions.Http
                // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/http-requests?view=aspnetcore-2.1
           
            services.AddRefitClient<IAuthApi>().ConfigureHttpClient(c => 
                c.BaseAddress = new Uri("https://my-secure-api.io/v1")
            ).AddPolicyHandler(retry)
                ;//.AddHttpMessageHandler<AutorizationHandler>();//.SetHandlerLifetime(TimeSpan.FromSeconds(49));
          
            //add service and then resolve it...
            services.AddTransient<IAuthService, AuthService>();
            
            services.AddTransient<AutorizationHandler>();
            
            var sp = services.BuildServiceProvider();
            AuthService = sp.GetService<IAuthService>();
            
            services.AddSingleton(Configuration);
            
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        private async Task<string> GetToken()
        {           
            await RefreshAuthorization();

            /*await Task.Run( () =>
            {
                Thread.Sleep(5000);
            });*/
                     
            return Token;
        }

        private async Task RefreshAuthorization()
        {
            var token = await AuthService.GetAccessToken();
            
            Token = "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJyeWFuQHRydWNrcXVpY2suY29tIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiYWRtaW4iLCJleHAiOjE1NDY0ODAwMzAsImlzcyI6IjgxM3NvZnR3YXJlLmNvbSIsImF1ZCI6IjgxM3NvZnR3YXJlLmNvbSJ9.oW_OAkjRccyAlvDx_EZcWR-v0j6avWByVGf-JRlQV0E";
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

    // ReSharper disable once ClassNeverInstantiated.Global
    public class AutorizationHandler: DelegatingHandler
    {
        private async Task<string> GetToken()
        {
            /*await Task.Run( () =>
            {
                Thread.Sleep(5000);
            });*/

            var token = await Startup.AuthService.GetAccessToken();
                     
            return "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJyeWFuQHRydWNrcXVpY2suY29tIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiYWRtaW4iLCJleHAiOjE1NDY0ODAwMzAsImlzcyI6IjgxM3NvZnR3YXJlLmNvbSIsImF1ZCI6IjgxM3NvZnR3YXJlLmNvbSJ9.oW_OAkjRccyAlvDx_EZcWR-v0j6avWByVGf-JRlQV0E";
        }
        
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.Remove("Authorization");
            //request.Headers.Add("Authorization", "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJyeWFuQHRydWNrcXVpY2suY29tIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiYWRtaW4iLCJleHAiOjE1NDY0ODAwMzAsImlzcyI6IjgxM3NvZnR3YXJlLmNvbSIsImF1ZCI6IjgxM3NvZnR3YXJlLmNvbSJ9.oW_OAkjRccyAlvDx_EZcWR-v0j6avWByVGf-JRlQV0E");
            request.Headers.Add("Authorization", await GetToken());
            return await base.SendAsync(request, cancellationToken);
        }
    }
}