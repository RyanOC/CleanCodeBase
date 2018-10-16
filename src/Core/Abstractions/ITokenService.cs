using System.Threading.Tasks;
using Core.Entities;

namespace Core.Abstractions
{
    public interface ITokenService
    {
        Task<string> AuthenticateAsync(LoginCredentials loginCredentials);
    }
}