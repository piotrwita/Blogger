using Infrastructure.Data;
using WebAPI.HealthChecks;

namespace WebAPI.Installers
{
    public class HealthChecksInstallers : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddHealthChecks()
                .AddDbContextCheck<BloggerContext>("Database");

            //rejestracja niestandardwego heathchecka
            services.AddHealthChecks()
                .AddCheck<ResponseTimeHealthCheck>("Network speed test");

            services.AddHealthChecksUI()
                .AddInMemoryStorage(); //wskazuje ze dane wyswietlane w ramach interfejsu uzytkownika beda przechowywane w pamieci
        }
    }
}
