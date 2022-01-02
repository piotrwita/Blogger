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

//co istotne musimy wywo³aæ middleware na samym pocz¹tku
app.UseMiddleware<ErrorHandlingMiddleware>();

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
//app.UseEndpoints(endpoints =>
//{
//    //w³¹czamy us³ugê wstrzykiwania zale¿noœci dla tras http
//    endpoints.EnableDependencyInjection();
//    //definiujemy mo¿liwe do wykonania operacje
//    endpoints.Filter().OrderBy().MaxTop(10);
//    //definiujemy routing. Jaki model bêdzie reprezentowany przez jak¹ encjê
//    endpoints.MapODataRoute("odata", "odata", GetEdmModel());
//});

app.Run();

//static IEdmModel GetEdmModel()
//{
//    var builder = new ODataConventionModelBuilder();
//    //wskazujemy, ¿e encjê Posts, bêdzie reprezentowa³ model PostDto
//    builder.EntitySet<PostDto>("Posts");

//    return builder.GetEdmModel();
//}