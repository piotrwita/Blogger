//using Microsoft.AspNet.OData.Builder;
//using Microsoft.AspNet.OData.Extensions;
using WebAPI.Installers;
using WebAPI.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.InstallServicesInAssembly(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseDeveloperExceptionPage();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebAPI v1"));
}

//co istotne musimy wywo�a� middleware na samym pocz�tku
app.UseMiddleware<ErrorHandlingMiddleware>();

//przekierowuje ��dania inne ni� https do adresu url https
app.UseHttpsRedirection();

//dodaje dopasowanie trasy czyli nawigacje do odpowiednich akcji kontrolera
app.UseRouting();
//umo�liwia ko�ystanie z uwie�ytelniania
app.UseAuthentication();
//umo�liwia ko�ystanie z autoryzacji
app.UseAuthorization();

app.MapControllers();

//dodaje wykonanie punktu ko�cowego do potoku ��da�
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