namespace DocumentFlowAPI.Services.Tasks.Dto;

public class TaskDetailsDto : TaskDto
{
    public Dictionary<string, object> Data { get; set; } = new();
    public string? DocumentNumber { get; set; }
    public int? Progress { get; set; }
    public string? ProgressMessage { get; set; }
}
