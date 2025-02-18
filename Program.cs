using Microsoft.EntityFrameworkCore;
using UserPermissionsAdmin.Data;
using UserPermissionsAdmin.Services;
using Serilog;
using UserPermissionsAdmin.Middleware;
using UserPermissionsAdmin.Repositories;
using Elastic.Clients.Elasticsearch;
using Elastic.Transport;

var builder = WebApplication.CreateBuilder(args);

// Elasticsearch connection string
var elasticUri = builder.Configuration.GetValue<string>("Elasticsearch:Uri");

if (string.IsNullOrEmpty(elasticUri))
{
    throw new Exception("Elasticsearch URI is not set.");
}

Console.WriteLine($"Elasticsearch URI: {elasticUri}");

var settings = new ElasticsearchClientSettings(new Uri(elasticUri))
                .CertificateFingerprint("33:55:28:87:30:88:C8:4A:94:E7:FD:C7:C9:66:3F:9F:BB:11:A9:6A:3E:9B:73:E8:DB:D7:C0:5B:DE:F7:81:75")
                .Authentication(new BasicAuthentication("elastic", "Elastic2025"))
                .DefaultIndex("permissions");

var elasticClient = new ElasticsearchClient(settings);

// Register the elastic client and ElasticSearchService
builder.Services.AddSingleton(elasticClient);
builder.Services.AddScoped<IElasticSearchService, ElasticSearchService>();

// Add services to the container.

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DataBaseConnection")));

Console.WriteLine($"Db password: {builder.Configuration.GetConnectionString("DataBaseConnection")}");

builder.Services.AddScoped<IPermissionRepository, PermissionRespository>();
builder.Services.AddScoped<IPermissionService, PermissionService>();
builder.Services.AddScoped<IPermissionTypeRepository, PermissionTypeRespository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddControllers().AddNewtonsoftJson();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Host.UseSerilog((context, services, configuration) => 
        configuration.WriteTo.Console());

var app = builder.Build();

app.UseMiddleware<RequestLoggingMiddleware>();
app.UseMiddleware<ExceptionHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

try
{
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application failed to start");
}
finally
{
    Log.CloseAndFlush();
}

public partial class Program() { }