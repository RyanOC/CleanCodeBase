using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;
using Refit;

namespace Core.Abstractions
{
    public interface IWebApi
    {
        [Get("/api/v1/customers")]
        //Task<IList<Customer>> GetCustomers([Header("Authorization")] string authorization);
        
        Task<IList<Customer>> GetCustomers();
    }
}