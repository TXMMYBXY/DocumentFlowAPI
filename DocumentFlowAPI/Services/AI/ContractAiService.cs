using OllamaSharp;

namespace DocumentFlowAPI.Services.AI;

public class ContractAiService : IContractAiService
{
    private readonly OllamaApiClient _ollama;

    public ContractAiService(OllamaApiClient ollama)
    {
        _ollama = ollama;
        _ollama.SelectedModel = "deepseek-v3.1:671b-cloud";
    }

    public async Task<string> ExtractFieldsJsonAsync(string contractText)
    {
        string prompt = $$"""
        Ты — эксперт по анализу договоров. Проанализируй текст договора/заявления и **верни строго валидный JSON** в формате ниже **без любых пояснений, комментариев или текста вокруг**. Например где встретится много подряд пробелов, значит это место для заполнения данных. 
        И оставь в этом файле метки для себя, потому что в будущем ты по этим меткам будешь заполнять эти поля данными, которые тебе передадут после ответа. 
        Учти, что во многих документах требуется ввести реквизиты контрагента, такие как ИНН, КПП, ОГРН, адрес, телефон, email и т.д. — это все поля для заполнения. При этом обычно реквизиты одной строрны указаны, поэтому ты можешь использвоать их как подсказку
        JSON должен содержать массив полей для заполнения пустых значений в договоре. Твой ответ будет использоваться в дальнейшем в коде, поэтому строго как в примере JSON далее:  

        {
          [
            {
              "key": "string",
              "title": "string",
              "type": "text | date | number | select",
              "options": ["string"]?
            }
          ]
        }

        Вот класс по которому нужно формировать ответ: 
        public class TemplateFieldInfoDto
        {
            [JsonPropertyName("key")]
            public string Key { get; set; } = null!;
            [JsonPropertyName("title")]
            public string Title { get; set; } = null!;
            [JsonPropertyName("type")]
            public string Type { get; set; } = "string";
            [JsonPropertyName("required")]
            public bool Required { get; set; }
            [JsonPropertyName("options")]
            public List<string>? Options { get; set; }
        }

        пример:
        {
          [
            {
              "key": "contract_number",
              "title": "Номер договора",
              "type": "text",
            }
          ]
        }
        Возьми текст договора ниже и извлеки из него только данные для полей:

        {{contractText}}
        """;


        string result = "";

        await foreach (var chunk in _ollama.GenerateAsync(prompt))
        {
            result += chunk.Response;
        }
        Console.WriteLine("checkpoint");
        // result += _ollama.SelectedModel;
        return result;
    }
}
