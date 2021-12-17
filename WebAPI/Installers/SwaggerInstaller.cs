using Microsoft.OpenApi.Models;

namespace WebAPI.Installers
{
    public class SwaggerInstaller : IInstaller

    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwaggerGen(c =>
            {
                c.EnableAnnotations();
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Blogger API", Version = "v1" });
            });
        }
    }
}
