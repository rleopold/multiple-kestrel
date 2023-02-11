using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

public class KestrelBackgroundService : BackgroundService
{
    private IWebHost _webHost;

    public KestrelBackgroundService()
{
            _webHost = WebHost.CreateDefaultBuilder()
                .UseStartup<Startup>()
                .UseKestrel()
                .UseUrls("http://localhost:8002")
                .Build();
}
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _webHost.StartAsync();
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        await _webHost.StopAsync();
        await base.StopAsync(cancellationToken);
    }
}