using Microsoft.AspNetCore.Http;

namespace Modules.Shared.Application.IService;

public interface IEmailService
{
    Task SendMailAsync(string mailTo, string subject, string body, IList<IFormFile>? files = null);

}