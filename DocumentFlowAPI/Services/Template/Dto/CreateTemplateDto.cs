using System.ComponentModel.DataAnnotations.Schema;
using DocumentFlowAPI.Services.User;

namespace DocumentFlowAPI.Services.Template.Dto;

public class CreateTemplateDto
{
    public string Title { get; set; }
    public bool IsActive { get; set; }
    public int CreatedBy { get; set; } = UserIdentity.User!.Id;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string FileName { get; set; }
    public long FileLength { get; set; }
    public Stream FileStream { get; set; }
}
