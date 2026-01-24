using DocumentFlowAPI.Interfaces.Services;
using DocumentFlowAPI.Services.WorkerTask.Dto;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace DocumentFlowAPI.Services.Template;

public class FieldExtractorService : IFieldExtractorService
{
    public async Task<IReadOnlyList<TemplateFieldInfoDto>> ExtractFieldsAsync(string templatePath)
    {
        templatePath = _NormalizePath(templatePath);

        await using var fileStream = File.Open(
            templatePath,
            FileMode.Open,
            FileAccess.Read,
            FileShare.Read);

        await using var mem = new MemoryStream();
        await fileStream.CopyToAsync(mem);
        mem.Position = 0;

        using var doc = WordprocessingDocument.Open(mem, false);

        var result = new List<TemplateFieldInfoDto>();

        var contentControls = doc
            .MainDocumentPart!
            .Document
            .Descendants<SdtElement>();

        foreach (var sdt in contentControls)
        {
            var props = sdt.SdtProperties;
            if (props == null) continue;

            var tag = props.GetFirstChild<Tag>()?.Val?.Value;
            if (string.IsNullOrWhiteSpace(tag)) continue;

            var title = props.GetFirstChild<SdtAlias>()?.Val?.Value;

            var type =
                sdt is SdtRun ? "string" :
                sdt is SdtBlock ? "multiline" :
                "string";

            result.Add(new TemplateFieldInfoDto
            {
                Key = tag,
                Title = title ?? tag,
                Type = type,
                Required = true
            });
        }

        return result
            .GroupBy(x => x.Key)
            .Select(g => g.First())
            .ToList();
    }

    private static string _NormalizePath(string path)
    {
        return path
            .Trim()
            .Replace("\u202A", "")
            .Replace("\u202B", "")
            .Replace("\u202C", "")
            .Replace("\u200E", "")
            .Replace("\u200F", "")
            .Replace("\uFEFF", "");
    }
}
