using System.Threading.Tasks;
using Core.Entities;
using Refit;

namespace Core.Abstractions
{
    public interface IAuthApi
    {
        [Post("/api/token")]
        Task<AccessToken> GetAccessToken([Body] AuthCredentials request);
    }
}