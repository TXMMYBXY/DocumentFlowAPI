using AutoMapper;
using DocumentFlowAPI.Controllers.Auth;
using DocumentFlowAPI.Controllers.Template.ViewModels;
using DocumentFlowAPI.Enums;
using DocumentFlowAPI.Interfaces.Services;
using DocumentFlowAPI.Models;
using DocumentFlowAPI.Services.Template;
using DocumentFlowAPI.Services.Template.Dto;
using Microsoft.AspNetCore.Mvc;

namespace DocumentFlowAPI.Controllers.Template;

[ApiController]
[Route("api/statement-template")]
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
    [AuthorizeByRoleId]
    [HttpGet("{templateId}")]
    public async Task<ActionResult<GetTemplateViewModel>> GetTemplateById([FromRoute] int templateId)
    {
        var templateDto = await _templateService.GetTemplateForWorkerByIdAsync<StatementTemplate>(templateId);
        var templateViewModel = _mapper.Map<GetTemplateViewModel>(templateDto);

        return Ok(templateViewModel);
    }

    /// <summary>
    /// Получение списка шаблонов заявлений
    /// </summary>
    [AuthorizeByRoleId]
    [HttpGet]
    public async Task<ActionResult<PagedTemplateViewModel>> GetAllTemplates([FromQuery] TemplateFilter templateFilter)
    {
        var templatesDto = await _templateService.GetAllTemplatesAsync<StatementTemplate>(templateFilter);
        var templatesViewModel = _mapper.Map<PagedTemplateViewModel>(templatesDto);

        return Ok(templatesViewModel);
    }

    /// <summary>
    /// Добавление нового шаблона заявлений
    /// </summary>
    [AuthorizeByRoleId((int)Permissions.Admin, (int)Permissions.Boss)]
    [HttpPost]
    public async Task<ActionResult> CreateTemplate([FromForm] CreateTemplateViewModel templateViewModel)
    {
        var templateDto = _mapper.Map<CreateTemplateDto>(templateViewModel);

        await _templateService.CreateTemplateAsync<StatementTemplate>(templateDto);

        return Ok();
    }

    /// <summary>
    /// Смена статуса шаблона
    /// </summary>
    /// <param name="templateId"></param>
    /// <returns></returns>
    [AuthorizeByRoleId((int)Permissions.Admin, (int)Permissions.Boss)]
    [HttpPatch("{templateId}/change-template-status")]
    public async Task<ActionResult<bool>> ChangeTemplateStatus([FromRoute] int templateId)
    {
        var status = await _templateService.ChangeTemplateStatusById<StatementTemplate>(templateId);

        return Ok(status);
    }

    /// <summary>
    /// Удаление шаблона заявлений
    /// </summary>
    [AuthorizeByRoleId((int)Permissions.Admin, (int)Permissions.Boss)]
    [HttpDelete("{templateId}")]
    public async Task<ActionResult> DeleteTemplate([FromRoute] int templateId)
    {
        await _templateService.DeleteTemplateAsync<StatementTemplate>(templateId);

        return Ok();
    }

    /// <summary>
    /// Удаление нескольких шаблонов заявлений
    /// </summary>
    [AuthorizeByRoleId((int)Permissions.Admin, (int)Permissions.Boss)]
    [HttpDelete]
    public async Task<ActionResult> DeleteManyTemplates([FromBody] DeleteManyTemplatesViewModel deleteTemplateViewModel)
    {
        await _templateService.DeleteManyTemplatesAsync<StatementTemplate>(deleteTemplateViewModel.TemplateIds);

        return Ok();
    }

    /// <summary>
    /// Обновление шаблона заявления
    /// </summary>
    [AuthorizeByRoleId((int)Permissions.Admin, (int)Permissions.Boss)]
    [HttpPatch("{templateId}/update-template")]
    public async Task<ActionResult> UpdateTemplateByIdPartial([FromRoute] int templateId, 
        [FromForm] UpdateTemplateViewModel templateViewModel)
    {
        var templateDto = _mapper.Map<UpdateTemplateDto>(templateViewModel);
        
        await _templateService.UpdateTemplatePartialAsync<StatementTemplate>(templateId, templateDto);

        return Ok();
    }

    [AuthorizeByRoleId]
    [HttpGet("{templateId}/extract-fields")]
    public async Task<ActionResult<IReadOnlyList<TemplateFieldInfoViewModel>>> ExtractFields([FromRoute] int templateId)
    {
        var resultDto = await _templateService.ExtractFieldsFromTemplateAsync<StatementTemplate>(templateId);
        var resultViewModel = _mapper.Map<IReadOnlyList<TemplateFieldInfoViewModel>>(resultDto);

        return Ok(resultViewModel);
    }

    [AuthorizeByRoleId]
    [HttpGet("{templateId:int}/download")]
    public async Task<IActionResult> GetDocument([FromRoute] int templateId)
    {
        var templateDto = await _templateService.DownloadTemplateAsync<StatementTemplate>(templateId);

        var templateViewModel = _mapper.Map<DownloadTemplateViewModel>(templateDto);

        var stream = new FileStream(templateViewModel.FilePath, FileMode.Open, FileAccess.Read);

        return File(stream, "application/octet-stream", templateViewModel.FileName);
    }
}
