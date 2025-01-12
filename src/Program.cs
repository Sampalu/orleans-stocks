using Stocks;

var builder = WebApplication.CreateBuilder();

builder.Host
    .UseOrleans(siloBuilder =>
    {
        siloBuilder.UseLocalhostClustering();
        siloBuilder.UseDashboard(options => { options.Port = 8081; });
    })
    .ConfigureServices(
        services => services.AddHostedService<StocksHostedService>());

var app = builder.Build();

app.MapGet("/", () => "Welcome to the Stock Sample, powered by Orleans!");

app.MapGet("/health", () => Results.Ok(new { status = "Healthy", timestamp = DateTime.UtcNow }));

app.Run();
