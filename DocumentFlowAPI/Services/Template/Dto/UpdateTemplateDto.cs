namespace DocumentFlowAPI.Services.Template.Dto;

public class UpdateTemplateDto
{
    public string? Title { get; set; }
    public string? FileName { get; set; }
    public long? FileLength { get; set; }
    public Stream? FileStream { get; set; }
}
