//using Microsoft.AspNet.OData.Builder;
//using Microsoft.AspNet.OData.Extensions;
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

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

//app.UseEndpoints(endpoints =>
//{
//    //w��czamy us�ug� wstrzykiwania zale�no�ci dla tras http
//    endpoints.EnableDependencyInjection();
//    //definiujemy mo�liwe do wykonania operacje
//    endpoints.Filter().OrderBy().MaxTop(10);
//    //definiujemy routing. Jaki model b�dzie reprezentowany przez jak� encj�
//    endpoints.MapODataRoute("odata", "odata", GetEdmModel());
//});

app.Run();

//static IEdmModel GetEdmModel()
//{
//    var builder = new ODataConventionModelBuilder();
//    //wskazujemy, �e encj� Posts, b�dzie reprezentowa� model PostDto
//    builder.EntitySet<PostDto>("Posts");

//    return builder.GetEdmModel();
//}