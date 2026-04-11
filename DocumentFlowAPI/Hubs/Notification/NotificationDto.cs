using DocumentFlowAPI.Enums;

namespace DocumentFlowAPI.Hubs.Notification;

public sealed record NotificationDto
(
    NotificationKind Kind,
    NotificationSeverity Severity,
    string Title,
    string Message
);