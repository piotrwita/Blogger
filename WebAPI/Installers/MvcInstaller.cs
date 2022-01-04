using Application;
using Application.Services;
using Application.Validators;
using FluentValidation.AspNetCore;
using Infrastructure;
//using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Mvc.Versioning;
using WebAPI.Middlewares;

namespace WebAPI.Installers
{
    public class MvcInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddApplication();
            services.AddInfrastructure();

            services.AddControllers()
                .AddFluentValidation(options =>
                {
                    //automatycznie zarejestruje wszystkie walidatory z projektu w ktorym znajduje sie wskazany typ
                    //co za tym idzie wywołane w momencie wystapienia walidacji danych okreslonego typu
                    options.RegisterValidatorsFromAssemblyContaining<CreatePostDtoValidatior>();
                })
                .AddJsonOptions(options =>
                {
                    //konfiguracja pozwala wyświetlać w przeglądarce wynik w bardziej czytelny sposób
                    options.JsonSerializerOptions.WriteIndented = true;
                });

            services.AddApiVersioning(x =>
            {
                x.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
                //jeżeli klient nie poda w żądaniu wersji interfejsu api to zostanie użyta wersja domyślna
                x.AssumeDefaultVersionWhenUnspecified = true;
                //dodaje nagłówek odpowiedzi API Supported version
                x.ReportApiVersions = true;
                //ustawienie nazwy parametru odpowiedzialnego za wersję API
                x.ApiVersionReader = new HeaderApiVersionReader("x-api-version");
            });

            services.AddAuthorization();

            services.AddTransient<UserResolverService>();

            services.AddScoped<ErrorHandlingMiddleware>();

            //services.AddOData();
        }
    }
}
