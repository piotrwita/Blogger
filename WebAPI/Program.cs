//using Microsoft.AspNet.OData.Builder;
//using Microsoft.AspNet.OData.Extensions;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using NLog.Web;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using WebAPI.Installers;
using WebAPI.Middlewares;

WebApplicationBuilder builder;

//dzieki temu mozemy zalogowac akcje podczas procesu tworzenia aplikacji
var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();

try
{
    builder = WebApplication.CreateBuilder(args);
}
catch (Exception ex)
{
    logger.Fatal(ex, "API stopped");
    throw;
}
finally
{
    //reczne zwolnienie zasobow klasy logera
    NLog.LogManager.Shutdown();
}


//builder.Host.UseNLog();
builder.Host.UseSerilog((context, configuration) =>
{
    //umozliwia rejestrowanie dodadkowych wartosci do loga
    configuration.Enrich.FromLogContext()
    .Enrich.WithMachineName()
    .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(context.Configuration["ElasticConfiguration:Uri"]))
    {
        IndexFormat = $"{context.Configuration["ApplicationName"]}-logs-{context.HostingEnvironment.EnvironmentName?.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyu-MM}",
        AutoRegisterTemplate = true,
        NumberOfShards = 2,
        NumberOfReplicas = 1
    })
    .Enrich.WithProperty("Enviroment", context.HostingEnvironment.EnvironmentName)
    .ReadFrom.Configuration(context.Configuration);
});

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

//co istotne musimy wywo³aæ middleware na samym pocz¹tku
app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseMiddleware<RequestTimeMiddleware>();

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

//przekierowuje ¿¹dania inne ni¿ https do adresu url https
app.UseHttpsRedirection();

//dodaje dopasowanie trasy czyli nawigacje do odpowiednich akcji kontrolera
app.UseRouting();
//umo¿liwia ko¿ystanie z uwie¿ytelniania
app.UseAuthentication();
//umo¿liwia ko¿ystanie z autoryzacji
app.UseAuthorization();

app.MapControllers();

//dodaje wykonanie punktu koñcowego do potoku ¿¹dañ
app.UseEndpoints(endpoints =>
{
    ////w³¹czamy us³ugê wstrzykiwania zale¿noœci dla tras http
    //endpoints.EnableDependencyInjection();
    ////definiujemy mo¿liwe do wykonania operacje
    //endpoints.Filter().OrderBy().MaxTop(10);
    ////definiujemy routing. Jaki model bêdzie reprezentowany przez jak¹ encjê
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
//    //wskazujemy, ¿e encjê Posts, bêdzie reprezentowa³ model PostDto
//    builder.EntitySet<PostDto>("Posts");

//    return builder.GetEdmModel();
//}