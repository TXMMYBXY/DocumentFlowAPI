namespace DocumentFlowAPI.Services.Tasks.Dto;

public class TaskCancelDto
{
    public int UserId { get; set; }
    public string? Reason { get; set; }
}
