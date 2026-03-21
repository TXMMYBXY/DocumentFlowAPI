using System.Text.Json.Serialization;

namespace DocumentFlowAPI.Controllers.Role.ViewModels;

public class GetRoleViewModel
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }
}