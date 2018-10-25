using System.Net;
using System.Threading.Tasks;
using Core.Abstractions;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace WebApi.Controllers
{
    [Produces("application/json")]
    public class TokenController : Controller
    {
        private IConfiguration _configuration;
        private readonly ITokenService _tokenService;
        
        public TokenController(IConfiguration Configuration, ITokenService tokenService)
        {
            _configuration = Configuration;
            _tokenService = tokenService;
        }
        
        [Route("api/v1/token")]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]LoginCredentials request)
        {
            if (request.UserName == string.Empty || request.Password == string.Empty)
            {
                return Forbid("Login Failed");
            }

            var token = await _tokenService.AuthenticateAsync(request);
            
            return Ok(token);
        }
    }
}