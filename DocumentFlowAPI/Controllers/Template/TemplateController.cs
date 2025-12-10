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
///Этим контроллером могут пользоваться все, за исключением конкретных методов
public class TemplateController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly ITemplateService _templateService;

    public TemplateController(IMapper mapper, ITemplateService templateService)
    {
        _mapper = mapper;
        _templateService = templateService;
    }

    /// <summary>
    /// Только для сотрудников отдела закупок
    /// Получение шаблона договора по id
    /// </summary>
    /// <returns>ViewModel шаблона</returns>
    [HttpGet("{templateId}/contract-template")]
    public async Task<ActionResult<GetTemplateViewModel>> GetContractTemplateById([FromRoute] int templateId)
    {
        var templateDto = await _templateService.GetTemplateByIdAsync<Models.ContractTemplate>(templateId);
        var templateViewModel = _mapper.Map<GetTemplateViewModel>(templateDto);

        return Ok(templateViewModel);
    }

    /// <summary>
    /// Только для сотрудников отдела закупок
    /// Получение списка шаблонов договоров
    /// </summary>
    /// <returns>Список шаблонов</returns>
    [HttpGet("all-contract-templates")]
    public async Task<ActionResult<List<GetTemplateViewModel>>> GetAllContractTemplatesAsync()
    {
        var templatesDto = await _templateService.GetAllTemplatesAsync<Models.ContractTemplate>();
        var templatesViewModel = _mapper.Map<List<GetTemplateViewModel>>(templatesDto);

        return Ok(templatesViewModel);
    }

    /// <summary>
    /// Только для главы отдела закупок
    /// Добавляет новый шаблон договора из тела NewTemplateViewModel
    /// </summary>
    [HttpPost("add-contract-template")]
    public async Task<ActionResult> CreateContractTemplate([FromBody] CreateTemplateViewModel templateViewModel)
    {
        var templateDto = _mapper.Map<CreateTemplateDto>(templateViewModel);

        await _templateService.CreateTemplateAsync<Models.ContractTemplate>(templateDto);

        return Ok();
    }

    /// <summary>
    /// Только для главы отдел закупок
    /// Удаляет шаблон договора из тела
    /// </summary>
    /// <param name="templateViewModel"></param>
    /// <returns></returns>
    [HttpDelete("delete-contract-template")]
    public async Task<ActionResult> DeleteContractTemplateById([FromBody] DeleteTemplateViewModel templateViewModel)
    {
        await _templateService.DeleteTemplateAsync<Models.ContractTemplate>(templateViewModel.TemplateId);

        return Ok();
    }

    /// <summary>
    /// Только для главы отдела закупок
    /// Изменяет шаблон договора
    /// </summary>
    [HttpPatch("{templateId}/update-contract-template")]
    public async Task<ActionResult> UpdateContractTemplateById([FromRoute] int templateId, [FromBody] UpdateTemplateViewModel templateViewModel)
    {
        var templateDto = _mapper.Map<UpdateTemplateDto>(templateViewModel);

        await _templateService.UpdateTemplateAsync<Models.ContractTemplate>(templateId, templateDto);

        return Ok();
    }

    /// <summary>
    /// Получение шаблона заявления
    /// </summary>
    [HttpGet("{templateId}/statement-template")]
    public async Task<ActionResult<GetTemplateViewModel>> GetStatementTemplateById([FromRoute] int templateId)
    {
        var templateDto = await _templateService.GetTemplateByIdAsync<Models.StatementTemplate>(templateId);
        var templateViewModel = _mapper.Map<GetTemplateViewModel>(templateDto);

        return Ok(templateViewModel);
    }

    /// <summary>
    /// Получение списка шаблонов заявлений
    /// </summary>
    [HttpGet("all-statement-templates")]
    public async Task<ActionResult<List<GetTemplateViewModel>>> GetAllStatementTemplatesAsync()
    {
        var templatesDto = await _templateService.GetAllTemplatesAsync<Models.StatementTemplate>();
        var templatesViewModel = _mapper.Map<List<GetTemplateViewModel>>(templatesDto);

        return Ok(templatesViewModel);
    }

    /// <summary>
    /// Только для главы отдела закупок
    /// Добавление нового шаблона заявлений
    /// </summary>
    [HttpPost("add-statement-template")]
    public async Task<ActionResult> CreateStatementTemplate([FromBody] CreateTemplateViewModel templateViewModel)
    {
        var templateDto = _mapper.Map<CreateTemplateDto>(templateViewModel);

        await _templateService.CreateTemplateAsync<Models.StatementTemplate>(templateDto);

        return Ok();
    }

    /// <summary>
    /// Только для главы отдела закупок
    /// Удаление шаблона заявлений
    /// </summary>
    [HttpDelete("{templateId}/delete-statement-template")]
    public async Task<ActionResult> DeleteStatementTemplateById([FromRoute] int templateId)
    {
        await _templateService.DeleteTemplateAsync<Models.StatementTemplate>(templateId);

        return Ok();
    }

    /// <summary>
    /// Только для главы отдела закупок
    /// Обновление шаблона заявления
    /// </summary>
    [HttpPatch("{templateId}/update-statement-template")]
    public async Task<ActionResult> UpdateStatementTemplateById([FromRoute] int templateId, [FromBody] UpdateTemplateViewModel templateViewModel)
    {
        var templateDto = _mapper.Map<UpdateTemplateDto>(templateViewModel);

        await _templateService.UpdateTemplateAsync<Models.StatementTemplate>(templateId, templateDto);

        return Ok();
    }
}