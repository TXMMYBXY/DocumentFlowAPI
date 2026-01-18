using System.ComponentModel.DataAnnotations;

namespace DocumentFlowAPI.Services.Tasks.Dto;

public class TaskProgressDto
{
    [Range(0, 100)]
    public int Progress { get; set; }

    [MaxLength(127)]
    public string Message { get; set; } = string.Empty;
}
