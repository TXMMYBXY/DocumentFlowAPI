using DocumentFlowAPI.Enums;
using DocumentFlowAPI.Models;
using TaskStatus = DocumentFlowAPI.Enums.TaskStatus;

namespace DocumentFlowAPI.Controllers.Tasks.ViewModels;

public class TaskFilterViewModel
{
    public TaskStatus? Status { get; set; }
    public TemplateType? TemplateType { get; set; }
    public int? UserId { get; set; }
    public TaskPriority? Priority { get; set; }
    public DateTime? CreatedFrom { get; set; }
    public DateTime? CreatedTo { get; set; }
    public DateTime? UpdatedFrom { get; set; }
    public DateTime? UpdatedTo { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public string? SortBy { get; set; } = "CreatedAt";
    public bool SortDescending { get; set; } = true;
}
