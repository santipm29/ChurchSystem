using ChurchSystem.Common.Models;
using ChurchSystem.Common.Responses;
using System.IO;
using System.Threading.Tasks;

namespace ChurchSystem.Common.Services
{
    public interface IApiService
    {

        Task<Response> GetListAsync<T>(string urlBase, string servicePrefix, string controller);

        Task<RandomUsers> GetRandomUser(string urlBase, string servicePrefix);

        Task<Stream> GetPictureAsync(string urlBase, string servicePrefix);
    }
}
