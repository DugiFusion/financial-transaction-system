using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Transaction.Data;
using Transactions.Repositories;
using Transactions.Repositories.Interfaces;

var builder = WebApplication.CreateBuilder(args);

Console.WriteLine($"**********************************************************\n" +
                  $"**********************************************************\n\n" +
                  $"STARTING TRANSACTION SERVICE IN {builder.Environment.EnvironmentName} MODE\n\n" +
                  $"**********************************************************\n" +
                  $"**********************************************************\n");





builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true);


// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

//builder.Services.AddEndpointsApiExplorer();


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        config => config
            .AllowAnyOrigin()
            // .WithOrigins("http://localhost:4200") // Frontend url
            .AllowAnyMethod()
            .AllowAnyHeader());
});



builder.Services.AddDbContext<TransactionContext>(options =>
{
    options.UseSqlServer("Server=DUGI;Database=TransactionsDB;Trusted_Connection=True;TrustServerCertificate=True;"); // "Server=DUGI\\SQLEXPRESS;Database=TransactionsDB;Trusted_Connection=True;TrustServerCertificate=True;"
});


builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<TransactionContext>();


builder.Services.AddControllers();
builder.Services.AddSwaggerGen(s =>
{
    s.SwaggerDoc("v1", new OpenApiInfo { Title = "Transaction.API", Version = "v1" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Transaction.API v1"));
}

app.UseCors("AllowSpecificOrigin");


app.MapControllers();

app.UseHttpsRedirection();



app.Run();

