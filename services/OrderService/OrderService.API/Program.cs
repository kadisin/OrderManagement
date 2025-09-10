using Infrastructure.Persistence;
using MediatR;
using OrderService.Application.Commands;
using OrderService.Application.Interfaces;
using OrderService.Infrastructure.Messaging;
using OrderService.Infrastructure.Persistence;
using System.Reflection;
using Azure.Messaging.ServiceBus;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .Enrich.FromLogContext()
    .Enrich.WithMachineName()
    .Enrich.WithProcessId()
    .Enrich.WithThreadId()
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog();

// --- OpenTelemetry / Azure Monitor (Application Insights) ---
var aiConnectionString = configuration["ApplicationInsights:ConnectionString"];
if (string.IsNullOrWhiteSpace(aiConnectionString))
{
    // Optionally fallback to env var APPLICATIONINSIGHTS_CONNECTION_STRING
    aiConnectionString = Environment.GetEnvironmentVariable("APPLICATIONINSIGHTS_CONNECTION_STRING");
}

// Configure resource
var serviceName = "OrderService";
var resourceBuilder = ResourceBuilder.CreateDefault().AddService(serviceName);

// Add tracing & metrics with Azure Monitor exporter
builder.Services.AddOpenTelemetry()
    .WithTracing(tracerProviderBuilder =>
    {
        tracerProviderBuilder
            .SetResourceBuilder(resourceBuilder)
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddEntityFrameworkCoreInstrumentation() // if you use EF
            .AddAzureMonitorTraceExporter(o =>
            {
                o.ConnectionString = aiConnectionString;
            });
    })
    .WithMetrics(meterProviderBuilder =>
    {
        meterProviderBuilder
            .SetResourceBuilder(resourceBuilder)
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddRuntimeInstrumentation()
            .AddAzureMonitorMetricExporter(o =>
            {
                o.ConnectionString = aiConnectionString;
            });
});


// Azure Service Bus
string serviceBusConnection = builder.Configuration.GetConnectionString("ServiceBus");
builder.Services.AddSingleton(new ServiceBusClient(serviceBusConnection));
builder.Services.AddScoped<IMessagePublisher, AzureServiceBusPublisher>();

// Database context
builder.Services.AddDbContext<OrderDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// MediatR + DI
builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
builder.Services.AddMediatR(typeof(CreateOrderCommandHandler).Assembly);
builder.Services.AddSingleton<IRepository, Repository>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthorization();
app.MapControllers();

try
{
    Log.Information("Starting {Service}", serviceName);
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}