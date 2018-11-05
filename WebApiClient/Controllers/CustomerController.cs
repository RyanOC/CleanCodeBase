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

            var customers = await _webApi.GetCustomers();
            
            return Ok(customers);
        }
    }
}