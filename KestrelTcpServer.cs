using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Connections;

public class KestrelTcpServer : BackgroundService
{
    private IWebHost _webHost;

    public KestrelTcpServer()
    {
        _webHost = WebHost.CreateDefaultBuilder()
            .Configure(app => { })
            .UseKestrel(options =>
            {
                options.ListenAnyIP(8001, builder =>
                {
                    builder.UseConnectionHandler<MyEchoConnectionHandler>();
                });
            }).Build();

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

public class MyEchoConnectionHandler : ConnectionHandler
{
    private readonly ILogger<MyEchoConnectionHandler> _logger;
    public MyEchoConnectionHandler(ILogger<MyEchoConnectionHandler> logger)
    {
        _logger = logger;
    }

    public override async Task OnConnectedAsync(ConnectionContext connection)
    {
        _logger.LogInformation(connection.ConnectionId + " connected");

        while (true)
        {
            var result = await connection.Transport.Input.ReadAsync();
            var buffer = result.Buffer;

            foreach (var segment in buffer)
            {
                await connection.Transport.Output.WriteAsync(segment);
            }

            if (result.IsCompleted)
            {
                break;
            }

            connection.Transport.Input.AdvanceTo(buffer.End);
        }
        
        _logger.LogInformation(connection.ConnectionId + " disconnected");
    }
}