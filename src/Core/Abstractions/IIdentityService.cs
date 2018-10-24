using System.Threading.Tasks;
using Core.Entities;

namespace Core.Abstractions
{
    public interface IIdentityService
    {
        Task<string> AuthenticateAsync(LoginCredentials loginCredentials);
    }
}