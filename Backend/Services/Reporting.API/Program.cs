using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using RabbitMQ.Client;
using Reporting.API.EventBusConsumer;
using Reporting.API.Repositories;
using Reporting.API.Repositories.Interfaces;
using Transaction.Data;

var builder = WebApplication.CreateBuilder(args);

Console.WriteLine($"**********************************************************\n" +
                  $"**********************************************************\n\n" +
                  $"STARTING REPORTING SERVICE IN {builder.Environment.EnvironmentName} MODE\n\n" +
                  $"**********************************************************\n" +
                  $"**********************************************************\n");

builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true);

builder.Services.AddOpenApi();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        config => config
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

var connectionString = builder.Configuration.GetConnectionString("ReportingDatabase");
builder.Services.AddDbContext<ReportsContext>(options =>
{
    options.UseSqlServer(connectionString);
});
builder.Services.AddDbContext<ReportFilesContext>(options =>
{
    options.UseSqlServer(connectionString);
});


builder.Services.AddScoped<IReportRepository, ReportRepository>();

// RABBITMQ
builder.Services.AddSingleton<IConnectionFactory>(sp => new ConnectionFactory
{
    HostName = "localhost",
    UserName = "guest",
    Password = "guest",
    Port = 5672
});

builder.Services.AddTransient<Consumer>();
builder.Services.AddScoped<MessageHandler>();

builder.Services.AddControllers();
builder.Services.AddSwaggerGen(s =>
{
    s.SwaggerDoc("v1", new OpenApiInfo { Title = "Reporting.API", Version = "v1" });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Reporting.API v1"));
}

app.UseCors("AllowSpecificOrigin");

app.MapControllers();

app.UseHttpsRedirection();

// RabbitMQ Consumer
// RabbitMQ Consumer
var scope = app.Services.CreateScope();

var scopedServices = scope.ServiceProvider;
var rabbitMqConsumer = scopedServices.GetRequiredService<Consumer>();
var messageHandler = scopedServices.GetRequiredService<MessageHandler>();

rabbitMqConsumer.StartConsuming("transactionQueue", messageHandler.HandleMessage);



app.Run();