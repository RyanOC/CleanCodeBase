using System.Threading.Tasks;
using Core.Entities;

namespace Core.Abstractions
{
    public interface IAuthService
    {
        Task<AccessToken> GetAccessToken();
    }
}