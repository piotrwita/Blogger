using Infrastructure.Data.Dapper;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Installers
{
    public class DapperInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("BloggerCS");

            services.AddSingleton<IDapperContext>(x => new DapperContext(connectionString));
        }
    }
}
