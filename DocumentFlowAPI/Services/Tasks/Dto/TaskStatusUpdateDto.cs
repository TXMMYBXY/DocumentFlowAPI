using System.ComponentModel.DataAnnotations;

namespace DocumentFlowAPI.Services.Tasks.Dto;

public class TaskStatusUpdateDto
{
    [EnumDataType(typeof(TaskStatus))]
    public TaskStatus Status { get; set; }

    [StringLength(511)]
    public string? ResultFilePath { get; set; }
    public string? ErrorMessage { get; set; }
}
