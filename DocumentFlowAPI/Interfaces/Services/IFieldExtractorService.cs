using DocumentFlowAPI.Services.WorkerTask.Dto;

namespace DocumentFlowAPI.Interfaces.Services;

public interface IFieldExtractorService
{
    Task<IReadOnlyList<TemplateFieldInfoDto>> ExtractFieldsAsync(string templatePath);
}
