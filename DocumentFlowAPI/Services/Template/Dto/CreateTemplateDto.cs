using System.ComponentModel.DataAnnotations.Schema;
using DocumentFlowAPI.Services.User;

namespace DocumentFlowAPI.Services.Template.Dto;

public class CreateTemplateDto
{
    public string Title { get; set; }
    public string Path { get; set; }
    [ForeignKey(nameof(CreatedBy))]
    public int CreatedBy { get; set; } = UserIdentity.User!.Id;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsActive { get; set; }
}
