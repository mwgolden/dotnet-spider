using message_bus;
using message_bus.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using RabbitMQ.Client;
using scheduler_api;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
string rmqConnection = builder.Configuration.GetConnectionString("RabbitMQ");
string quartzdb = builder.Configuration.GetConnectionString("quartznetdb");

builder.Services.AddDbContext<QuartznetContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("quartznetdb"));
});
builder.Services.AddSingleton<IMessageBus, RabbitMQBus>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo{Title = "Scheduler API", Version = "v1"});
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Scheduler API V1");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();