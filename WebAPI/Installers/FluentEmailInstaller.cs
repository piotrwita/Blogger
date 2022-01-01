using Application.Interfaces;
using Application.Services.Emails;

namespace WebAPI.Installers
{
    public class FluentEmailInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            var fromEmail = configuration["FluentEmail:FromEmail"];
            var fromName = configuration["FluentEmail:FromName"];
            var host = configuration["FluentEmail:SmtpSender:Host"];
            var port = int.Parse(configuration["FluentEmail:SmtpSender:Port"]);
            var username = configuration["FluentEmail:SmtpSender:Username"];
            var password = configuration["FluentEmail:SmtpSender:Password"];

            services
                .AddFluentEmail(fromEmail, fromName)
                //wykorzystywanie szablonów cs html podczas wysyłania wiadomości za pomocą fluent email
                .AddRazorRenderer()
                .AddSmtpSender(host, port, username, password);

            services.AddScoped<IEmailSenderService, EmailSenderService>();
        }
    }
}
