﻿using Application.Interfaces;
using Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.AddScoped<IPostService, PostService>();
            services.AddScoped<IPictureService, PictureService>();
            services.AddScoped<ICosmosPostService, CosmosPostService>();

            return services;
        }
    }
}
