using System.Text.Json.Serialization;

namespace DocumentFlowAPI.Controllers.User.ViewModels;

public class DeleteManyUsersViewModel
{
    [JsonPropertyName("userIds")]
    public List<int> UserIds { get; set; }
}
