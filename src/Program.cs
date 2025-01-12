using Orleans.Configuration;
using Stocks;
using System.Net;

var builder = WebApplication.CreateBuilder();

builder.Host
    .UseOrleans(siloBuilder =>
    {
        siloBuilder.UseLocalhostClustering();
        siloBuilder.Configure<ClusterOptions>(options =>
        {
            options.ClusterId = "dev";
            options.ServiceId = "dev-enrichment-service";
        });
        //siloBuilder.AddDynamoDBGrainStorageAsDefault(op =>
        //{
        //    op.TableName = "dev-orleans-state";
        //    op.Service = config.GetValue<string>("aws:region");
        //});
        //siloBuilder.UseDynamoDBClustering(op =>
        //{
        //    op.TableName = "dev-orleans-cluster";
        //    op.Service = config.GetValue<string>("aws:region");
        //});
        //siloBuilder.Configure<EndpointOptions>(options =>
        //{
        //    // since we are using awsvpc each container gets its own dns and ip
        //    var ip = Dns.GetHostAddressesAsync(Dns.GetHostName()).Result.First();
        //    options.AdvertisedIPAddress = ip;
        //    // These 2 ports will be used by a cluster
        //    // for silo to silo communications
        //    options.SiloPort = EndpointOptions.DEFAULT_SILO_PORT;
        //    // Port to use for the gateway (client to silo)
        //    options.GatewayPort = EndpointOptions.DEFAULT_GATEWAY_PORT;
        //    // Internal ports which you expose to docker
        //    options.GatewayListeningEndpoint = new IPEndPoint(IPAddress.Any, EndpointOptions.DEFAULT_GATEWAY_PORT);
        //    options.SiloListeningEndpoint = new IPEndPoint(IPAddress.Any, EndpointOptions.DEFAULT_SILO_PORT);
        //});
        siloBuilder.UseDashboard(options => { options.Port = 8081; });
    })    
    .ConfigureServices(
        services => services.AddHostedService<StocksHostedService>());

var app = builder.Build();

app.MapGet("/", () => "Welcome to the Stock Sample, powered by Orleans!");

app.MapGet("/health", () => Results.Ok(new { status = "Healthy", timestamp = DateTime.UtcNow }));

app.Run();
