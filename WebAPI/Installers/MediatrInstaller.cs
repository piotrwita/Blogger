using MediatR;

namespace WebAPI.Installers
{
    public class MediatrInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            //parametr zapewnia to, ze przeskanujemy caly projekt w poszukiwaniu handlerow
            //i automatycznie zarejestrujemy je w domyslnym kontenerze DI 
            //services.AddMediatR(typeof(Program));
        }
    }
}
