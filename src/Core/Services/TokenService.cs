using System.Threading;
using System.Threading.Tasks;
using Core.Abstractions;
using Core.Entities;

namespace Core.Services
{
    public class TokenService: ITokenService
    {
        public async Task<string> AuthenticateAsync(LoginCredentials loginCredentials)
        {
            //var idProvider = Task.Delay(2000);       
            //await idProvider;            
            //return true;       
            Thread.Sleep(1000); //just to simulate an http call delay
            return await Task.Run(() => "thisisatesttokenthatisnotvalid"); //hardcode token for now
        }
    }
}