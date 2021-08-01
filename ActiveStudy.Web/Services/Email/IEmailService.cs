using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace ActiveStudy.Web.Services.Email
{
    public interface IEmailService
    {
        Task SendEmailAsync(EmailRecipient recipient, string subject, string body);
    }
    
    public class NullEmailService : IEmailService
    {
        private readonly ILogger<NullEmailService> logger;

        public NullEmailService(ILogger<NullEmailService> logger)
        {
            this.logger = logger;
        }

        public Task SendEmailAsync(EmailRecipient recipient, string subject, string body)
        {
            logger.LogInformation($"Send email to {recipient.Name} (${recipient.Email}). Subject: {subject}. Body: {body}");
            return Task.CompletedTask;
        }
    }
}
