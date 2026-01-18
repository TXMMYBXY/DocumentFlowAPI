using DocumentFlowAPI.Models;

namespace DocumentFlowAPI.Services.Tasks.Dto;

public class TaskDto
{
    public Guid TaskId { get; set; }
    public TemplateType TemplateType { get; set; }
    public int TemplateId { get; set; }
    public Models.TaskStatus Status { get; set; }
    public TaskPriority Priority { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public string? ResultFilePath { get; set; }
    public string? ErrorMessage { get; set; }
    public int? UserId { get; set; }
    public int RetryCount { get; set; }
}
