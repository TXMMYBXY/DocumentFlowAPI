using DocumentFlowAPI.Hubs.Notification;
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

    public async Task SendNotificationToRoleAsync(string[] roleIds, NotificationDto notificationDto)
    {
        _logger.LogDebug("Notification {Kind} {Severity} {Title} {Message}",
            notificationDto.Kind, notificationDto.Severity, notificationDto.Title, notificationDto.Message);
        foreach (var roleId in roleIds)
        {
            await _notificationHub.Clients.Group(roleId).SendAsync("Notification", notificationDto);
        }
    }

    public async Task SendNotificationToUserAsync(string userId, NotificationDto notificationDto)
    {
        _logger.LogDebug("Notification {Kind} {Severity} {Title} {Message}",
            notificationDto.Kind, notificationDto.Severity, notificationDto.Title, notificationDto.Message);
        
        await _notificationHub.Clients.User(userId).SendAsync("Notification", notificationDto);
    }
}