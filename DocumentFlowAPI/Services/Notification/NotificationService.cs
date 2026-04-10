using DocumentFlowAPI.Hubs.Notifiaction;
using DocumentFlowAPI.Interfaces.Services;
using Microsoft.AspNetCore.SignalR;

namespace DocumentFlowAPI.Services.Notification;

public class NotificationService : INotificationService
{
    private readonly ILogger<NotificationService> _logger;
    private readonly IHubContext<NotificationHub> _notificationHub;

    public NotificationService(ILogger<NotificationService> logger, IHubContext<NotificationHub> notificationHub)
    {
        _logger = logger;
        _notificationHub = notificationHub;
    }
    
    public async Task SendNotificationToAllAsync(NotificationDto notificationDto)
    {
        _logger.LogDebug("Notification {Kind} {Severity} {Title} {Message}",
            notificationDto.Kind, notificationDto.Severity, notificationDto.Title, notificationDto.Message);
        
        await _notificationHub.Clients.All.SendAsync("Notification", notificationDto);
    }
}