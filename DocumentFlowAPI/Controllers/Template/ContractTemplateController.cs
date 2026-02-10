using AutoMapper;
using DocumentFlowAPI.Controllers.Auth;
using DocumentFlowAPI.Controllers.Template.ViewModels;
using DocumentFlowAPI.Interfaces.Services;
using DocumentFlowAPI.Models;
using DocumentFlowAPI.Services.Template.Dto;
using Microsoft.AspNetCore.Mvc;

namespace DocumentFlowAPI.Controllers.Template;

[ApiController]
[Route("api/contract-templates")]
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
    [AuthorizeByRoleId((int)Permissions.Boss, (int)Permissions.Purchaser)]
    [HttpGet("{templateId}/get-template")]
    public async Task<ActionResult<GetTemplateViewModel>> GetTemplateById([FromRoute] int templateId)
    {
        var templateDto = await _templateService.GetTemplateByIdAsync<ContractTemplate>(templateId);
        var templateViewModel = _mapper.Map<GetTemplateViewModel>(templateDto);

        return Ok(templateViewModel);
    }

    /// <summary>
    /// Только для сотрудников отдела закупок
    /// Получение списка шаблонов договоров
    /// </summary>
    /// <returns>Список шаблонов</returns>
    [AuthorizeByRoleId((int)Permissions.Boss, (int)Permissions.Purchaser)]
    [HttpGet("get-all")]
    public async Task<ActionResult<List<GetTemplateViewModel>>> GetAllTemplatesAsync()
    {
        var templatesDto = await _templateService.GetAllTemplatesAsync<ContractTemplate>();
        var templatesViewModel = _mapper.Map<List<GetTemplateViewModel>>(templatesDto);

        return Ok(templatesViewModel);
    }

    /// <summary>
    /// Только для главы отдела закупок
    /// Добавляет новый шаблон договора из тела NewTemplateViewModel
    /// </summary>
    [HttpPost("add-template")]
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
    [HttpPatch("{templateId}/change-template-status")]
    public async Task<ActionResult<bool>> ChangeTemplateStatus([FromRoute] int templateId)
    {
        var status = await _templateService.ChangeTemplateStatusById<ContractTemplate>(templateId);

        return Ok(status);
    }

    /// <summary>
    /// Только для главы отдел закупок
    /// Удаляет шаблон договора из тела
    /// </summary>
    /// <param name="templateViewModel"></param>
    /// <returns></returns>
    [HttpDelete("delete-template")]
    public async Task<ActionResult> DeleteTemplateById([FromBody] DeleteTemplateViewModel templateViewModel)
    {
        await _templateService.DeleteTemplateAsync<ContractTemplate>(templateViewModel.TemplateId);

        return Ok();
    }

    /// <summary>
    /// Только для главы отдела закупок
    /// Изменяет шаблон договора
    /// </summary>
    [HttpPatch("{templateId}/update-template")]
    public async Task<ActionResult> UpdateTemplateById([FromRoute] int templateId, [FromBody] UpdateTemplateViewModel templateViewModel)
    {
        var templateDto = _mapper.Map<UpdateTemplateDto>(templateViewModel);

        await _templateService.UpdateTemplateAsync<ContractTemplate>(templateId, templateDto);

        return Ok();
    }

    [HttpGet("{templateId}/extract-fields")]
    public async Task<ActionResult<IReadOnlyList<TemplateFieldInfoViewModel>>> ExctractFields([FromRoute] int templateId)
    {
        var resultDto = await _templateService.ExctractFieldsFromTemplateAsync<ContractTemplate>(templateId);
        var resultViewModel = _mapper.Map<IReadOnlyList<TemplateFieldInfoViewModel>>(resultDto);

        return Ok(resultViewModel);
    }
}