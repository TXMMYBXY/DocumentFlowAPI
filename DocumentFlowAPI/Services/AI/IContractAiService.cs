namespace DocumentFlowAPI.Services.AI;

public interface IContractAiService
{
    /// <summary>
    /// Анализирует текст договора и возвращает JSON со схемой полей
    /// </summary>
    /// <param name="contractText"></param>
    /// <returns>JSON string с полями для заполнения</returns>
    Task<string> ExtractFieldsJsonAsync(string contractText);
}

