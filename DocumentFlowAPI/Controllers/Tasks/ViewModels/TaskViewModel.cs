using DocumentFlowAPI.Models;

namespace DocumentFlowAPI.Controllers.Tasks.ViewModels;

public class TaskViewModel
{
    public Guid TaskId { get; set; }
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
    public bool CanRetry { get; set; }
    public bool CanCancel { get; set; }
    public bool CanDownload => Status == Models.TaskStatus.Completed && !string.IsNullOrEmpty(ResultFilePath);
    public string? DownloadUrl { get; set; }

}
