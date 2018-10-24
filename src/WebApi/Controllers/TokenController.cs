using System.Net;
using System.Threading.Tasks;
using Core.Abstractions;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Produces("application/json")]
    public class TokenController : Controller
    {
        private readonly ITokenService _tokenService;
        
        public TokenController(ITokenService tokenService)
        {
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
            
            return Ok(await _tokenService.AuthenticateAsync(request));
        }
    }
}