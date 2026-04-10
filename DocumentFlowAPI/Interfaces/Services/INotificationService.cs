using DocumentFlowAPI.Hubs.Notifiaction;

namespace DocumentFlowAPI.Interfaces.Services;

public interface INotificationService
{
    Task AddNewTemplateNotification(NotificationDto notificationDto);
}