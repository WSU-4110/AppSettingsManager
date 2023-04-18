using System.Text.Json;
using System.Text.Json.Serialization;
using AppSettingsManagerApi.Facades;
using AppSettingsManagerApi.Infrastructure.MySql;
using AppSettingsManagerApi.Infrastructure.MySql.Converters;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
});

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

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var certPath = Path.Combine(
    builder.Environment.ContentRootPath,
    "cert",
    "DigiCertGlobalRootCA.crt.pem"
);
connectionString = connectionString.Replace("{path_to_CA_cert}", certPath);

builder.Services.AddMySqlSettingsStorage(connectionString);

builder.Services.AddFacades();

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddAzureWebAppDiagnostics();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
