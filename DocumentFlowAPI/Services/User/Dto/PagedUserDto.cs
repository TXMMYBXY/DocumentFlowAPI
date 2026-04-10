using DocumentFlowAPI.Services.General;

namespace DocumentFlowAPI.Services.User.Dto;

public class PagedUserDto : PagedData
{
    public List<GetUserDto> Users { get; set; }
}
