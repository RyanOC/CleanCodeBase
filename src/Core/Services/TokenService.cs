using System.Threading.Tasks;
using Core.Abstractions;
using Core.Entities;

namespace Core.Services
{
    public class TokenService: ITokenService
    {
        private readonly IIdentityService _identityService ;

        public TokenService(IIdentityService identityService)
        {
            _identityService  = identityService ;
        }
        
        public async Task<string> AuthenticateAsync(LoginCredentials loginCredentials)
        { 
            return await _identityService .AuthenticateAsync(loginCredentials);
        }
    }
}