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

app.MapControllers();

app.Run();
