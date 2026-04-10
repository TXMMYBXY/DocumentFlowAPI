using DocumentFlowAPI.Services.General;

namespace DocumentFlowAPI.Services.Template.Dto;

public class PagedTemplateDto : PagedData
{
    public List<GetTemplateDto> Templates { get; set; }
}
