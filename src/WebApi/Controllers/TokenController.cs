using System;
using System.Threading.Tasks;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Produces("application/json")]
    public class TokenController : Controller
    {
        [Route("api/v1/token")]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]LoginCredentials request)
        {       
            return Content(String.Format("Welcome {0}", request.UserName));
        }
    }
}