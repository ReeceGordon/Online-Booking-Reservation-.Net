using Blazored.LocalStorage;
using Lotus.Client.AuthenticationStateProviders;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Lotus.Client
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddBlazoredLocalStorage();
            services.AddAuthorizationCore();
           // services.AddAuthenticationCore(); //Test
           // services.AddAuthentication(); // Test
            services.AddScoped<AuthenticationStateProvider, ApiAuthenticationStateProvider>();
            services.AddScoped<AuthService, AuthService>();
            
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            
            services.AddTransient<Services.BlazorTimer>();
        }

        public void Configure(IComponentsApplicationBuilder app)
        {
            
            app.AddComponent<App>("app");
        }
    }
}
