using Application;
using Infrastructure;
using Microsoft.AspNetCore.Mvc.Versioning;

namespace WebAPI.Installers
{
    public class MvcInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddApplication();
            services.AddInfrastructure();

            services.AddControllers();

            services.AddApiVersioning(x =>
            {
                x.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(2, 0);
                //jeżeli klient nie poda w żądaniu wersji interfejsu api to zostanie użyta wersja domyślna
                x.AssumeDefaultVersionWhenUnspecified = true;
                //dodaje nagłówek odpowiedzi API Supported version
                x.ReportApiVersions = true;
                //ustawienie nazwy parametru odpowiedzialnego za wersję API
                x.ApiVersionReader = new HeaderApiVersionReader("x-api-version");
            });
        }
    }
}
