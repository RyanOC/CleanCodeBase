using System.Threading.Tasks;
using Core.Abstractions;
using Core.Entities;

namespace WebApiClient
{
    public class AuthService: IAuthService
    {
        public AuthService(IAuthApi authApi)
        {
            _authApi = authApi;
        }
 
        private readonly IAuthApi _authApi;

        public async Task<AccessToken> GetAccessToken()
        {
            var authCredentials = new AuthCredentials
            {
                
            };

            var token = await _authApi.GetAccessToken(authCredentials);
            
            return token;
        }
    }
}