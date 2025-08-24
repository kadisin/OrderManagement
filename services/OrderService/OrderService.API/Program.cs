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

app.Run();