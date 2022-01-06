//using Microsoft.AspNet.OData.Builder;
//using Microsoft.AspNet.OData.Extensions;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Newtonsoft.Json;
using WebAPI.HealthChecks;
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

//jako argument przedstawiamy sciezke do endpointa przedstawiajacego kondycje naszego api
//ponizsze opcje pozwalaja na wyswietlenie w formacie json info o kondycji api reprezentowanej przez klase HealthCheckResponse
//app.UseHealthChecks("/health", new HealthCheckOptions
//{
//    ResponseWriter = async (context, report) =>
//    {
//        context.Response.ContentType = "application/json";

//        var response = new HealthCheckResponse
//        {
//            Status = report.Status.ToString(),
//            Checks = report.Entries.Select(x => new HealthCheck
//            {
//                Component = x.Key,
//                Status = x.Value.Status.ToString(),
//                Description = x.Value.Description
//            }),
//            Duration = report.TotalDuration
//        };

//        await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
//    }
//});

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
app.UseEndpoints(endpoints =>
{
    ////w��czamy us�ug� wstrzykiwania zale�no�ci dla tras http
    //endpoints.EnableDependencyInjection();
    ////definiujemy mo�liwe do wykonania operacje
    //endpoints.Filter().OrderBy().MaxTop(10);
    ////definiujemy routing. Jaki model b�dzie reprezentowany przez jak� encj�
    //endpoints.MapODataRoute("odata", "odata", GetEdmModel());
    endpoints.MapControllers();
    //umozliwia wyswietlenie informacji o kontroli kondycji przy pomocy interfejsu uzytkownika
    endpoints.MapHealthChecks("/health", new HealthCheckOptions()
    {
        Predicate = _ => true,
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });
    endpoints.MapHealthChecksUI();
});

app.Run();

//static IEdmModel GetEdmModel()
//{
//    var builder = new ODataConventionModelBuilder();
//    //wskazujemy, �e encj� Posts, b�dzie reprezentowa� model PostDto
//    builder.EntitySet<PostDto>("Posts");

//    return builder.GetEdmModel();
//}