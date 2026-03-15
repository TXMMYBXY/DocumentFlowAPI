using DocumentFlowAPI.Services.WorkerTask.Dto;

namespace DocumentFlowAPI.Interfaces.Services;

public interface IFieldExtractorService
{
    Task<List<TemplateFieldInfoDto>> ExtractFieldsAsync(string templatePath);
}
