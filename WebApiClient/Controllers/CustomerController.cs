using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Abstractions;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace WebApiClient.Controllers
{
    public class CustomerController : Controller
    {
        public CustomerController(IWebApi webApi)
        {
            _webApi = webApi;
        }
 
        private readonly IWebApi _webApi;
        
        [Route("api/v1/customers")]
        [HttpGet]
        public async Task<ActionResult<IList<Customer>>> Get()
        {
            //var webApi = RestService.For<IWebApi>("http://127.0.0.1:5002");

            var customers = await _webApi.GetCustomers("Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJyeWFuQHRydWNrcXVpY2suY29tIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiYWRtaW4iLCJleHAiOjE1NDY0ODAwMzAsImlzcyI6IjgxM3NvZnR3YXJlLmNvbSIsImF1ZCI6IjgxM3NvZnR3YXJlLmNvbSJ9.oW_OAkjRccyAlvDx_EZcWR-v0j6avWByVGf-JRlQV0E");
            
            return Ok(customers);
        }
    }
}