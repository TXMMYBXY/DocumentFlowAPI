using DocumentFlowAPI.Enums;

namespace DocumentFlowAPI.Hubs.Notifiaction;

public sealed record NotificationDto
(
    NotificationKind Kind,
    NotificationSeverity Severity,
    string Title,
    string Message
);