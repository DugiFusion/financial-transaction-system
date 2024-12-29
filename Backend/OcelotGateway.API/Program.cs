using Microsoft.OpenApi.Models;
using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

Console.WriteLine($"**********************************************************\n" +
                  $"**********************************************************\n\n" +
                  $"STARTING OCELOT GATEWAY IN {builder.Environment.EnvironmentName} MODE\n\n" +
                  $"**********************************************************\n" +
                  $"**********************************************************\n");


// Add services to the container.


builder.Configuration
    .AddJsonFile("ocelot.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"ocelot.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true);





//builder.Configuration.AddJsonFile($"ocelot.json", optional: false, true);


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        config => config
            .AllowAnyOrigin()
            // .WithOrigins("http://localhost:4200") // Frontend url
            .AllowAnyMethod()
            .AllowAnyHeader());
});



builder.Services.AddOcelot(builder.Configuration)
    .AddCacheManager(x =>
    {
        x.WithDictionaryHandle();
    });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}


app.UseHttpsRedirection();

app.UseRouting();

app.UseCors("AllowSpecificOrigin");

app.MapControllers();



await app.UseOcelot();

app.Run();