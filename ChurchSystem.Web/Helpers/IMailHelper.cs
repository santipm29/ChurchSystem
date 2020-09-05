using ChurchSystem.Common.Responses;

namespace ChurchSystem.Web.Helpers
{
    public interface IMailHelper
    {
        Response SendMail(string to, string subject, string body);
    }
}
