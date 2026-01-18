using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DocumentFlowAPI.Models;

namespace DocumentFlowAPI.Services.Tasks.Dto;

public class TaskFilterDto
{
    public Models.TaskStatus? Status { get; set; }
    public TemplateType? TemplateType { get; set; }
    public int? UserId { get; set; }
    public TaskPriority? Priority { get; set; }
    public DateTime? CreatedFrom { get; set; }
    public DateTime? CreatedTo { get; set; }
    public DateTime? UpdatedFrom { get; set; }
    public DateTime? UpdatedTo { get; set; }

    [Range(1, 100)]
    public int Page { get; set; } = 1;
    
    [Range(1, 100)]
    public int PageSize { get; set; } = 20;
    public string? SortBy { get; set; } = nameof(TaskModel.CreatedAt);
    public bool SortDescending { get; set; } = true;
}
