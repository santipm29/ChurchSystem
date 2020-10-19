using ChurchSystem.Common.Models;
using ChurchSystem.Common.Request;
using ChurchSystem.Common.Responses;
using System.IO;
using System.Threading.Tasks;

namespace ChurchSystem.Common.Services
{
    public interface IApiService
    {

        Task<Response> GetListAsync<T>(string urlBase, string servicePrefix, string controller);
        Task<Response> GetListTokenAsync<T>(string urlBase, string servicePrefix, string controlle, string accessToken);
        Task<Response> GetTokenAsync(string urlBase, string servicePrefix, string controller, TokenRequest request);

        Task<Response> GetUserByEmail(string urlBase, string servicePrefix, string controller, string tokenType, string accessToken, EmailRequest request);
        Task<Response> PostAsync<T>(string urlBase, string servicePrefix, string controller, T model, string token);
        Task<Response> PostListAsync<T>(string urlBase, string servicePrefix, string controller, T model, string token);
        Task<Response> RecoverPasswordAsync(string urlBase, string servicePrefix, string controller, EmailRequest emailRequest);
        Task<Response> RegisterUserAsync(string urlBase, string servicePrefix, string controller, UserRequest userRequest);
        Task<Response> ModifyUserAsync(string urlBase, string servicePrefix, string controller, UserRequest userRequest, string token);
        Task<Response> ChangePasswordAsync(string urlBase, string servicePrefix, string controller, ChangePasswordRequest changePasswordRequest, string token);
        Task<RandomUsers> GetRandomUser(string urlBase, string servicePrefix);

        Task<Stream> GetPictureAsync(string urlBase, string servicePrefix);
        Task<Response> PostListAsync<T>(string url, string v1, string v2, MeetingRequest meetingRequest, string token);
    }
}
