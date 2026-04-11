using DocumentFlowAPI.Hubs.Notification;

namespace DocumentFlowAPI.Interfaces.Services;

public interface INotificationService
{
    Task SendNotificationToAllAsync(NotificationDto notificationDto);
    Task SendNotificationToUserAsync(string userId, NotificationDto notificationDto);
    Task SendNotificationToRoleAsync(string[] roleIds, NotificationDto notificationDto);

}