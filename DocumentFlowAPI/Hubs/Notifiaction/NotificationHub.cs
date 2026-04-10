using Microsoft.AspNetCore.SignalR;

namespace DocumentFlowAPI.Hubs.Notifiaction;

public class NotificationHub : Hub
{
    private ILogger<NotificationHub> _logger;

    public NotificationHub(ILogger<NotificationHub> logger)
    {
        _logger = logger;
    }
    
    public override async Task OnConnectedAsync()
    {
        var connectionId = Context.ConnectionId;

        _logger.LogInformation($"Подключился: {connectionId}");

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var connectionId = Context.ConnectionId;

        _logger.LogInformation($"Отключился: {connectionId}");

        await base.OnDisconnectedAsync(exception);
    }
}