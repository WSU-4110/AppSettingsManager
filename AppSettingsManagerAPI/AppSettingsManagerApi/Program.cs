using AppSettingsManagerApi.Facades;
using AppSettingsManagerApi.Infrastructure.MySql;
using AppSettingsManagerApi.Infrastructure.MySql.Converters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Configuration.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json");

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc(
        "v1",
        new Microsoft.OpenApi.Models.OpenApiInfo
        {
            Title = "App Settings Manager v1",
            Version = "v1"
        }
    );
});

builder.Services.AddConverters();

builder.Services.AddMySqlSettingsStorage(
    builder.Configuration.GetConnectionString("DefaultConnection")
);

builder.Services.AddFacades();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
