using System.Text.Json.Serialization;
using DocumentFlowAPI.Services.General;

namespace DocumentFlowAPI.Controllers.User.ViewModels;

public class PagedUserViewModel : PagedData
{
    [JsonPropertyName("users")]
    public List<GetUserViewModel> Users { get; set; }
}
