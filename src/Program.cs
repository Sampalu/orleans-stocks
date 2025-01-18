using Microsoft.AspNetCore.ResponseCompression;
using Orleans.Configuration;
using Stocks;
using System.Net;

var builder = WebApplication.CreateBuilder();

builder.Host
        .UseOrleans(siloBuilder =>
        {
            siloBuilder.Configure<ClusterOptions>(options =>
            {
                options.ClusterId = "dev";
                options.ServiceId = "dev-enrichment-service";
            });
            siloBuilder.UseRedisClustering("localhost:6379,password=redispass,abortConnect=false");
            siloBuilder.Configure<EndpointOptions>(options =>
            {
                // since we are using awsvpc each container gets its own dns and ip
                var ip = Dns.GetHostAddressesAsync(Dns.GetHostName()).Result.First();
                options.AdvertisedIPAddress = ip;
            
                Random rdn = new Random();
                int siloPort = rdn.Next(EndpointOptions.DEFAULT_SILO_PORT, 12000);
                int gatewayPort = rdn.Next(EndpointOptions.DEFAULT_GATEWAY_PORT, 31000);

                // These 2 ports will be used by a cluster
                // for silo to silo communications
                options.SiloPort = siloPort;
                // Port to use for the gateway (client to silo)
                options.GatewayPort = gatewayPort;
                // Internal ports which you expose to docker
                options.GatewayListeningEndpoint = new IPEndPoint(IPAddress.Any, siloPort);
                options.SiloListeningEndpoint = new IPEndPoint(IPAddress.Any, gatewayPort);
            });
            siloBuilder.UseDashboard(options => { options.Port = 8081; });            
        })    
    .ConfigureServices(
        services => services.AddHostedService<StocksHostedService>());

var app = builder.Build();

app.MapGet("/", () => "Welcome to the Stock Sample, powered by Orleans!");

app.MapGet("/health", () => Results.Ok(new { status = "Healthy", timestamp = DateTime.UtcNow }));

app.Run();
