using Orleans.Runtime;
using Stocks;

var builder = WebApplication.CreateBuilder();

builder.Host
    .UseOrleans(siloBuilder =>
    {
        siloBuilder.UseLocalhostClustering();
        siloBuilder.UseDashboard(options => { });
    })
    .ConfigureServices(
        services => services.AddHostedService<StocksHostedService>());

var app = builder.Build();

app.MapGet("/", () => "Welcome to the Stock Sample, powered by Orleans!");

app.Run();
