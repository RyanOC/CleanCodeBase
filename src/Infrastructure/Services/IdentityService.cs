using System.Threading;
using System.Threading.Tasks;
using Core.Abstractions;
using Core.Entities;

namespace Infrastructure.Services
{
    public class IdentityService : IIdentityService
    {
        public async Task<string> AuthenticateAsync(LoginCredentials loginCredentials)
        {
            await Task.Run(() =>
            {
                Thread.Sleep(3000); // just to simulate a http identity provider service call delay
            });

            return "thisisatesttokenthatisnotvalid"; // hardcode token for now
        }
    }
}