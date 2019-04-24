namespace CHEJ_Shop.Web.Helpers
{
    using CHEJ_Shop.Common.Models;
    using System.Threading.Tasks;

    public interface IMailHelper
    {
        //  void SendMail(string to, string subject, string body);
        Task<Response> SendMail(string to, string nameTo, string subject, string body);
    }
}