using System.Threading.Tasks;

namespace BeerPal.Web.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
