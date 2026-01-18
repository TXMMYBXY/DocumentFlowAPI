using DocumentFlowAPI.Models;

namespace DocumentFlowAPI.Services.Tasks.Dto;

public class CreateTaskRequestDto
{
    public int TemplateId { get; set; }
    public TemplateType TemplateType { get; set; }
    public Dictionary<string, object> Data { get; set; } = new();
    public int? UserId { get; set; }
    public TaskPriority Priority { get; set; } = TaskPriority.Normal;
}
