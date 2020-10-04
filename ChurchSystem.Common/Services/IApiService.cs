using ChurchSystem.Common.Models;
using System.IO;
using System.Threading.Tasks;

namespace ChurchSystem.Common.Services
{
    public interface IApiService
    {
        Task<RandomUsers> GetRandomUser(string urlBase, string servicePrefix);

        Task<Stream> GetPictureAsync(string urlBase, string servicePrefix);
    }
}
