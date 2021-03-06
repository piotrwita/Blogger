using Application.Interfaces;
using Application.Services;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddMediatR(Assembly.GetExecutingAssembly());

            services.AddScoped<IPostService, PostService>();
            services.AddScoped<IPictureService, PictureService>();
            services.AddScoped<IAttachmentService, AttachmentService>();
            services.AddScoped<ICosmosPostService, CosmosPostService>();

            return services;
        }
    }
}
