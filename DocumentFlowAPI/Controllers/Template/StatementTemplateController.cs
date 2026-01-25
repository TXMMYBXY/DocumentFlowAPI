using AutoMapper;
using DocumentFlowAPI.Controllers.Auth;
using DocumentFlowAPI.Controllers.Template.ViewModels;
using DocumentFlowAPI.Interfaces.Services;
using DocumentFlowAPI.Models;
using DocumentFlowAPI.Services.Template.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DocumentFlowAPI.Controllers.Template;

[ApiController]
[Route("api/statement-templates")]
public class StatementTemplateController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly ITemplateService _templateService;

    public StatementTemplateController(IMapper mapper, ITemplateService templateService)
    {
        _mapper = mapper;
        _templateService = templateService;
    }

    /// <summary>
    /// Получение шаблона заявления
    /// </summary>
    [HttpGet("{templateId}/get-template")]
    public async Task<ActionResult<GetTemplateViewModel>> GetTemplateById([FromRoute] int templateId)
    {
        var templateDto = await _templateService.GetTemplateByIdAsync<StatementTemplate>(templateId);
        var templateViewModel = _mapper.Map<GetTemplateViewModel>(templateDto);

        return Ok(templateViewModel);
    }

    /// <summary>
    /// Получение списка шаблонов заявлений
    /// </summary>
    [HttpGet("get-all")]
    public async Task<ActionResult<List<GetTemplateViewModel>>> GetAllTemplatesAsync()
    {
        var templatesDto = await _templateService.GetAllTemplatesAsync<StatementTemplate>();
        var templatesViewModel = _mapper.Map<List<GetTemplateViewModel>>(templatesDto);

        return Ok(templatesViewModel);
    }

    /// <summary>
    /// Добавление нового шаблона заявлений
    /// </summary>
    [AuthorizeByRoleId((int)Permissions.Boss)]
    [HttpPost("add-template")]
    public async Task<ActionResult> CreateTemplate([FromBody] CreateTemplateViewModel templateViewModel)
    {
        var templateDto = _mapper.Map<CreateTemplateDto>(templateViewModel);

        await _templateService.CreateTemplateAsync<StatementTemplate>(templateDto);

        return Ok();
    }
    
    [AuthorizeByRoleId((int)Permissions.Boss)]
    [HttpPatch("{templateId}/change-template-status")]
    public async Task<ActionResult> ChangeTemplateStatus([FromRoute] int templateId)
    {
        await _templateService.ChangeTemplateStatusById<StatementTemplate>(templateId);

        return Ok();
    }

    /// <summary>
    /// Удаление шаблона заявлений
    /// </summary>
    [AuthorizeByRoleId((int)Permissions.Boss)]
    [HttpDelete("delete-template")]
    public async Task<ActionResult> DeleteTemplateById([FromBody] DeleteTemplateViewModel deleteTemplateViewModel)
    {
        await _templateService.DeleteTemplateAsync<StatementTemplate>(deleteTemplateViewModel.TemplateId);

        return Ok();
    }

    /// <summary>
    /// Обновление шаблона заявления
    /// </summary>
    [AuthorizeByRoleId((int)Permissions.Boss)]
    [HttpPatch("{templateId}/update-template")]
    public async Task<ActionResult> UpdateTemplateById([FromRoute] int templateId, [FromBody] UpdateTemplateViewModel templateViewModel)
    {
        var templateDto = _mapper.Map<UpdateTemplateDto>(templateViewModel);

        await _templateService.UpdateTemplateAsync<StatementTemplate>(templateId, templateDto);

        return Ok();
    }

    [Authorize]
    [HttpGet("{templateId}/extract-fields")]
    public async Task<ActionResult<IReadOnlyList<TemplateFieldInfoViewModel>>> ExctractFields([FromRoute] int templateId)
    {
        var resultDto = await _templateService.ExctractFieldsFromTemplateAsync<StatementTemplate>(templateId);
        var resultViewModel = _mapper.Map<IReadOnlyList<TemplateFieldInfoViewModel>>(resultDto);

        return Ok(resultViewModel);
    }
}
