using System.Threading.Tasks;
using ActiveStudy.Web.Services.Email;
using HandlebarsDotNet;
using Microsoft.Extensions.Localization;

namespace ActiveStudy.Web.Services
{
    public class NotificationManager
    {
        private readonly IEmailService emailService;
        private readonly IStringLocalizer<SharedResource> localizer;

        public NotificationManager(IEmailService emailService,
            IStringLocalizer<SharedResource> localizer)
        {
            this.emailService = emailService;
            this.localizer = localizer;
        }
        
        public async Task SendInvitationEmailAsync(InvitationEmailTemplateInfo info)
        {
            var subject = Handlebars.Compile(localizer["EmailTemplate_InvitationEmail_Subject"].Value)(info);
            var body = Handlebars.Compile(localizer["EmailTemplate_InvitationEmail_Body"].Value)(info);

            await emailService.SendEmailAsync(
                new EmailRecipient($"{info.User.FirstName} {info.User.LastName}", info.User.Email), subject, body);
        }

        public async Task SendEmailConfirmationAsync(EmailConfirmationTemplateInfo info)
        {
            var subject = Handlebars.Compile(localizer["EmailTemplate_EmailConfirmation_Subject"].Value)(info);
            var body = Handlebars.Compile(localizer["EmailTemplate_EmailConfirmation_Body"].Value)(info);

            await emailService.SendEmailAsync(
                new EmailRecipient($"{info.User.FirstName} {info.User.LastName}", info.User.Email), subject, body);
        }

        public async Task SendPasswordRecoveryAsync(PasswordRecoveryTemplateInfo info)
        {
            var subject = Handlebars.Compile(localizer["EmailTemplate_PasswordRecovery_Subject"].Value)(info);
            var body = Handlebars.Compile(localizer["EmailTemplate_PasswordRecovery_Body"].Value)(info);

            await emailService.SendEmailAsync(
                new EmailRecipient($"{info.User.FirstName} {info.User.LastName}", info.User.Email), subject, body);
        }
    }
}