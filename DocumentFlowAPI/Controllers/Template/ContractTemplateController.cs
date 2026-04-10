using AutoMapper;
using DocumentFlowAPI.Controllers.Auth;
using DocumentFlowAPI.Controllers.Template.ViewModels;
using DocumentFlowAPI.Interfaces.Services;
using DocumentFlowAPI.Models;
using DocumentFlowAPI.Services.Template;
using DocumentFlowAPI.Services.Template.Dto;
using Microsoft.AspNetCore.Mvc;

namespace DocumentFlowAPI.Controllers.Template;

[ApiController]
[Route("api/contract-template")]
[AuthorizeByRoleId((int)Permissions.Boss)]
public class ContractTemplateController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly ITemplateService _templateService;

    public ContractTemplateController(IMapper mapper, ITemplateService templateService)
    {
        _mapper = mapper;
        _templateService = templateService;
    }

    /// <summary>
    /// Только для сотрудников отдела закупок
    /// Получение шаблона договора по id
    /// </summary>
    /// <returns>ViewModel шаблона</returns>
    [AuthorizeByRoleId]
    [HttpGet("{templateId}/get-template")]
    public async Task<ActionResult<GetTemplateViewModel>> GetTemplateById([FromRoute] int templateId)
    {
        var templateDto = await _templateService.GetTemplateForWorkerByIdAsync<ContractTemplate>(templateId);
        var templateViewModel = _mapper.Map<GetTemplateViewModel>(templateDto);

        return Ok(templateViewModel);
    }

    /// <summary>
    /// Только для сотрудников отдела закупок
    /// Получение списка шаблонов договоров
    /// </summary>
    /// <returns>Список шаблонов</returns>
    [AuthorizeByRoleId((int)Permissions.Boss, (int)Permissions.Purchaser)]
    [HttpGet]
    public async Task<ActionResult<PagedTemplateViewModel>> GetAllTemplates([FromQuery] TemplateFilter templateFilter)
    {
        var templatesDto = await _templateService.GetAllTemplatesAsync<ContractTemplate>(templateFilter);
        var templatesViewModel = _mapper.Map<PagedTemplateViewModel>(templatesDto);

        return Ok(templatesViewModel);
    }

    /// <summary>
    /// Только для главы отдела закупок
    /// Добавляет новый шаблон договора из тела NewTemplateViewModel
    /// </summary>
    [AuthorizeByRoleId((int)Permissions.Admin, (int)Permissions.Boss)]
    [HttpPost]
    public async Task<ActionResult> CreateTemplate([FromBody] CreateTemplateViewModel templateViewModel)
    {
        var templateDto = _mapper.Map<CreateTemplateDto>(templateViewModel);

        await _templateService.CreateTemplateAsync<ContractTemplate>(templateDto);

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
        var status = await _templateService.ChangeTemplateStatusById<ContractTemplate>(templateId);

        return Ok(status);
    }

    /// <summary>
    /// Удаляет шаблон договора 
    /// </summary>
    /// <param name="templateViewModel"></param>
    /// <returns></returns>
    [AuthorizeByRoleId((int)Permissions.Admin, (int)Permissions.Boss)]
    [HttpDelete("{templateId}")]
    public async Task<ActionResult> DeleteTemplate([FromRoute] int templateId)
    {
        await _templateService.DeleteTemplateAsync<StatementTemplate>(templateId);

        return Ok();
    }

    /// <summary>
    /// Удаление нескольких шаблонов договоров
    /// </summary>
    /// <param name="deleteTemplateViewModel">Ids</param>
    [AuthorizeByRoleId((int)Permissions.Admin, (int)Permissions.Boss)]
    [HttpDelete]
    public async Task<ActionResult> DeleteManyTemplates([FromBody] DeleteManyTemplatesViewModel deleteTemplateViewModel)
    {
        await _templateService.DeleteManyTemplatesAsync<StatementTemplate>(deleteTemplateViewModel.TemplateIds);

        return Ok();
    }

    /// <summary>
    /// Только для главы отдела закупок
    /// Изменяет шаблон договора
    /// </summary>
    [AuthorizeByRoleId((int)Permissions.Admin, (int)Permissions.Boss)]
    [HttpPatch("{templateId}/update-template")]
    public async Task<ActionResult> UpdateTemplateById([FromRoute] int templateId, [FromBody] UpdateTemplateViewModel templateViewModel)
    {
        var templateDto = _mapper.Map<UpdateTemplateDto>(templateViewModel);

        await _templateService.UpdateTemplatePartialAsync<ContractTemplate>(templateId, templateDto);

        return Ok();
    }

    [AuthorizeByRoleId]
    [HttpGet("{templateId}/extract-fields")]
    public async Task<ActionResult<IReadOnlyList<TemplateFieldInfoViewModel>>> ExctractFields([FromRoute] int templateId)
    {
        var resultDto = await _templateService.ExtractFieldsFromTemplateAsync<ContractTemplate>(templateId);
        var resultViewModel = _mapper.Map<IReadOnlyList<TemplateFieldInfoViewModel>>(resultDto);

        return Ok(resultViewModel);
    }
}