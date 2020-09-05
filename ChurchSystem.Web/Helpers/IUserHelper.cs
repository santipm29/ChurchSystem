using ChurchSystem.Web.Data.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace ChurchSystem.Web.Helpers
{
    public interface IUserHelper
    {
        Task<User> GetUserAsync(string email);

        Task<User> GetUserAsync(Guid userId);

        Task<IdentityResult> AddUserAsync(User user, string password);

        Task CheckRoleAsync(string roleName);

        Task AddUserToRoleAsync(User user, string roleName);
        Task<bool> IsUserInRoleAsync(User user, string roleName);
    }
}
