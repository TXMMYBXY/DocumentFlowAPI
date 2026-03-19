namespace DocumentFlowAPI.Services.Template;

public class TemplateFilter
{
    public string? Title { get; set; }
    public int? CreatedBy { get; set; } 
    public DateTime? CreatedAt { get; set; }
    
    public int? PageSize { get; set; }
    public int? PageNumber { get; set; }
}