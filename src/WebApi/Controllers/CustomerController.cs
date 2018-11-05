using System;
using System.Collections.Generic;
using Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    public class CustomerController : Controller
    {
        [Route("api/v1/customers")]
        //[Authorize]
        [HttpGet]
        public ActionResult<IList<Customer>> Get()
        {
            var customers = new List<Customer>
            {
                new Customer
                {
                    Id = 1,
                    FirstName = "Michael",
                    LastName = "Faraday",
                    EmailAddress = "mfaraday@test.com",
                    DateCreated = new DateTime(2018, 10, 25),
                    DateModified = new DateTime(2018, 10, 25),
                    IsActive = true
                },
                new Customer
                {
                    Id = 2,
                    FirstName = "Sarah",
                    LastName = "Boysen",
                    EmailAddress = "sboysen@test.com",
                    DateCreated = new DateTime(2018, 10, 25),
                    DateModified = new DateTime(2018, 10, 25),
                    IsActive = true
                }
            };

            return customers;
        }
    }
}