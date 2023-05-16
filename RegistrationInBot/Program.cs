using Application;
using Persistence.YDB;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

services.AddSingleton<YDBContext>();
services.AddApplicationLayer();
services.AddSwaggerGen();

services.AddControllers();

services.AddCors(options =>
{
    options.AddDefaultPolicy(
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});

await YDBContext.Run();

var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI(config => {
    config.RoutePrefix = string.Empty;
    config.SwaggerEndpoint("swagger/v1/swagger.json", "RegistrationStudent API");
});

app.UseCors();
/*app.UseCors(builder =>
        builder.WithOrigins("http://localhost:3000"));*/

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
