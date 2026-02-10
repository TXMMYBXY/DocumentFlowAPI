using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentFlowAPI.Controllers.Template.ViewModels;

public class GetTemplateViewModel
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Path { get; set; }
    public int CreatedBy { get; set; }
    [ForeignKey(nameof(CreatedBy))]
    public virtual Models.User User{ get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsActive { get; set; }
}
