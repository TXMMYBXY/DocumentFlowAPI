using System.ComponentModel.DataAnnotations;
using DocumentFlowAPI.Models;

namespace DocumentFlowAPI.Controllers.Tasks.ViewModels;

public class CreateTaskRequestViewModel
{
    public int TemplateId { get; set; }

    [EnumDataType(typeof(TemplateType))]
    public TemplateType TemplateType { get; set; }
    public Dictionary<string, object> Data { get; set; } = new();

    [EnumDataType(typeof(TaskPriority))]
    public TaskPriority Priority { get; set; } = TaskPriority.Normal;
}
