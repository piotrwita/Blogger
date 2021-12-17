using Cosmonaut;
using Cosmonaut.Extensions.Microsoft.DependencyInjection;
using Domain.Entities.Cosmos;
using Microsoft.Azure.Documents.Client;

namespace WebAPI.Installers
{
    public class CosmosInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            var connectionPolicy = new ConnectionPolicy { 
                ConnectionMode = ConnectionMode.Direct, 
                ConnectionProtocol = Protocol.Tcp };
            
            var cosmosStoreSettings = new CosmosStoreSettings(
                configuration["CosmosSettings:DataBaseName"],
                configuration["CosmosSettings:AccountUri"],
                configuration["CosmosSettings:AccountKey"],
                connectionPolicy);

            services.AddCosmosStore<CosmosPost>(cosmosStoreSettings);
        }
    }
}
