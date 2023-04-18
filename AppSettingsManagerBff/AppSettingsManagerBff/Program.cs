using AppSettingsManagerBff.Infrastructure.ApiRepositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Configuration.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json");

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var appSettingsManagerApiConfig = builder.Configuration
    .GetSection("AppSettingsManagerApi")
    .Get<AppSettingsManagerApiConfig>();

builder.Services.AddApiRepositories(appSettingsManagerApiConfig);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
