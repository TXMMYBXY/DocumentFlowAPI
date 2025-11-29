using AutoMapper;
using DocumentFlowAPI.Controllers.Template.ViewModels;
using DocumentFlowAPI.Interfaces.Services;
using DocumentFlowAPI.Services.Template.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DocumentFlowAPI.Controllers.Template;

[ApiController]
[Route("api/templates")]
[Authorize]
public class TemplateController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly ITemplateService _templateService;

    public TemplateController(IMapper mapper, ITemplateService templateService)
    {
        _mapper = mapper;
        _templateService = templateService;
    }

    [HttpGet("{templateId}/contract-template")]
    public async Task<ActionResult<GetTemplateViewModel>> GetContractTemplateById([FromRoute] int templateId)
    {
        var templateDto = await _templateService.GetTemplateByIdAsync<Models.ContractTemplate>(templateId);
        var templateViewModel = _mapper.Map<GetTemplateViewModel>(templateDto);

        return Ok(templateViewModel);
    }

    [HttpGet("all-contract-templates")]
    public async Task<ActionResult<List<GetTemplateViewModel>>> GetAllContractTemplatesAsync()
    {
        var templatesDto = await _templateService.GetAllTemplatesAsync<Models.ContractTemplate>();
        var templatesViewModel = _mapper.Map<List<GetTemplateViewModel>>(templatesDto);

        return Ok(templatesViewModel);
    }

    [HttpPost("add-contract-template")]
    public async Task<ActionResult> CreateContractTemplate([FromBody] NewTemplateViewModel templateViewModel)
    {
        var templateDto = _mapper.Map<NewTemplateDto>(templateViewModel);

        await _templateService.CreateTemplateAsync<Models.ContractTemplate>(templateDto);

        return Ok();
    }

    [HttpDelete("{templateId}/delete-contract-template")]
    public async Task<ActionResult> DeleteContractTemplateById([FromRoute] int templateId)
    {
        await _templateService.DeleteTemplateAsync<Models.ContractTemplate>(templateId);

        return Ok();
    }

    [HttpPatch("{templateId}/update-contract-template")]
    public async Task<ActionResult> UpdateContractTemplateById([FromRoute] int templateId, [FromBody] UpdateTemplateViewModel templateViewModel)
    {
        var templateDto = _mapper.Map<UpdateTemplateDto>(templateViewModel);

        await _templateService.UpdateTemplateAsync<Models.ContractTemplate>(templateId, templateDto);

        return Ok();
    }

    [HttpGet("{templateId}/statement-template")]
    public async Task<ActionResult<GetTemplateViewModel>> GetStatementTemplateById([FromRoute] int templateId)
    {
        var templateDto = await _templateService.GetTemplateByIdAsync<Models.StatementTemplate>(templateId);
        var templateViewModel = _mapper.Map<TemplateViewModel>(templateDto);

        return Ok(templateViewModel);
    }

    [HttpGet("all-statement-templates")]
    public async Task<ActionResult<List<GetTemplateViewModel>>> GetAllStatementTemplatesAsync()
    {
        var templatesDto = await _templateService.GetAllTemplatesAsync<Models.StatementTemplate>();
        var templatesViewModel = _mapper.Map<List<GetTemplateViewModel>>(templatesDto);

        return Ok(templatesViewModel);
    }

    [HttpPost("add-statement-template")]
    public async Task<ActionResult> CreateStatementTemplate([FromBody] NewTemplateViewModel templateViewModel)
    {
        var templateDto = _mapper.Map<NewTemplateDto>(templateViewModel);

        await _templateService.CreateTemplateAsync<Models.StatementTemplate>(templateDto);

        return Ok();
    }

    [HttpDelete("{templateId}/delete-statement-template")]
    public async Task<ActionResult> DeleteStatementTemplateById([FromRoute] int templateId)
    {
        await _templateService.DeleteTemplateAsync<Models.StatementTemplate>(templateId);

        return Ok();
    }

    [HttpPatch("{templateId}/update-statement-template")]
    public async Task<ActionResult> UpdateStatementTemplateById([FromRoute] int templateId, [FromBody] UpdateTemplateViewModel templateViewModel)
    {
        var templateDto = _mapper.Map<UpdateTemplateDto>(templateViewModel);

        await _templateService.UpdateTemplateAsync<Models.StatementTemplate>(templateId, templateDto);

        return Ok();
    }
}