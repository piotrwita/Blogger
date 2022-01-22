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
    public class CorsInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                    //z metod naszego api moze skorzystac dowolna inna strona
                    .AllowAnyOrigin()
                    //adres plikacji ktora ma korzystac z naszego api
                    //.WithOrigins("http://localhost:3000")
                    //wykonanie dowolnej metody (post get put delete)
                    .AllowAnyMethod()
                    //tylko pewne metody
                    //.WithMethods("GET")
                    //przesyłanie dowolnego typu nagłówków
                    .AllowAnyHeader());
            });
        }
    }
}
