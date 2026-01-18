namespace DocumentFlowAPI.Services.Tasks.Dto;

public class TaskResultDto
{
    public Guid TaskId { get; set; }
    public TaskStatus Status { get; set; }
    public string? ResultFilePath { get; set; }
    public string? DownloadUrl { get; set; }
    public string? ErrorMessage { get; set; }
    public string Message { get; set; } = string.Empty;
}
