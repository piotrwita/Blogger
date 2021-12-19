using Application.Dto;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.OData.Edm;
using WebAPI.Installers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.InstallServicesInAssembly(builder.Configuration);

var app = builder.Build();

// configure the HTTP request pipeline.

app.UseDeveloperExceptionPage();

app.UseSwagger();

app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebAPI v1"));

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    //w³¹czamy us³ugê wstrzykiwania zale¿noœci dla tras http
    endpoints.EnableDependencyInjection();
    //definiujemy mo¿liwe do wykonania operacje
    endpoints.Filter().OrderBy().MaxTop(10);
    //definiujemy routing. Jaki model bêdzie reprezentowany przez jak¹ encjê
    endpoints.MapODataRoute("odata", "odata", GetEdmModel());

    endpoints.MapControllers();
});

app.Run();


static IEdmModel GetEdmModel()
{
    var builder = new ODataConventionModelBuilder();
    //wskazujemy, ¿e encjê Posts, bêdzie reprezentowa³ model PostDto
    builder.EntitySet<PostDto>("Posts");

    return builder.GetEdmModel();
}