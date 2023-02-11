using System.Net.Sockets;
using test;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<KestrelBackgroundService>();
        services.AddHostedService<KestrelTcpServer>();
        services.AddHostedService<Worker>();
    })
    .Build();

host.Run();



