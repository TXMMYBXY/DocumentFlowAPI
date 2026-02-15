using DocumentFlowAPI.Interfaces.Services;
using DocumentFlowAPI.Services.WorkerTask.Dto;
using DocumentFormat.OpenXml.Office2010.Word;
using DocumentFormat.OpenXml.Office2013.Word;
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

            string type = "string";
            List<string>? options = null;

            var dropDown = props.GetFirstChild<SdtContentDropDownList>();
            if (dropDown != null)
            {
                type = "dropdown";

                options = dropDown
                    .Elements<ListItem>()
                    .Select(i => i.DisplayText?.Value ?? i.Value?.Value)
                    .Where(v => !string.IsNullOrWhiteSpace(v))
                    .Distinct()
                    .ToList();
            }

            var comboBox = props.GetFirstChild<SdtContentComboBox>();
            if (comboBox != null)
            {
                type = "combobox";

                options = comboBox
                    .Elements<ListItem>()
                    .Select(i => i.DisplayText?.Value ?? i.Value?.Value)
                    .Where(v => !string.IsNullOrWhiteSpace(v))
                    .Distinct()
                    .ToList();
            }

            type = GetTypeOfProperty(props, type);

            result.Add(new TemplateFieldInfoDto
            {
                Key = tag,
                Title = title ?? tag,
                Type = type,
                Required = true,
                Options = options
            });
        }

            return result
            .GroupBy(x => x.Key)
            .Select(g => g.First())
            .ToList();
    }

    private static string GetTypeOfProperty(SdtProperties props, string type)
    {
        if (props.GetFirstChild<SdtContentDate>() != null)
        {
            type = "date";
        }

        if (props.GetFirstChild<SdtContentCheckBox>() != null)
        {
            type = "checkbox";
        }

        if (props.GetFirstChild<SdtContentRichText>() != null)
        {
            type = "richtext";
        }

        if (props.GetFirstChild<SdtContentText>() != null)
        {
            type = "text";
        }

        if (props.GetFirstChild<SdtContentPicture>() != null)
        {
            type = "picture";
        }

        if (props.GetFirstChild<SdtRepeatedSection>() != null)
        {
            type = "repeating-section";
        }

        return type;
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
