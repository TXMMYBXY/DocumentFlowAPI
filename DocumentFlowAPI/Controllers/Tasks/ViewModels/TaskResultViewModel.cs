namespace DocumentFlowAPI.Controllers.Tasks.ViewModels;

public class TaskResultViewModel
{
    public Guid TaskId { get; set; }
    public string Status { get; set; } = string.Empty;
    public string StatusDisplay { get; set; } = string.Empty;
    public string? ResultFilePath { get; set; }
    public string? DownloadUrl { get; set; }
    public string? ErrorMessage { get; set; }
    public string Message { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
