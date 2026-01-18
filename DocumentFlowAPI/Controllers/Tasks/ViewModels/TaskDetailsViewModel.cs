namespace DocumentFlowAPI.Controllers.Tasks.ViewModels;

public class TaskDetailsViewModel : TaskViewModel
{
    public Dictionary<string, object> Data { get; set; } = new();
    public string? DocumentNumber { get; set; }
    public int? Progress { get; set; }
    public string? ProgressMessage { get; set; }
}
