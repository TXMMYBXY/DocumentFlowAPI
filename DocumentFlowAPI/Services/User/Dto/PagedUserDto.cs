namespace DocumentFlowAPI.Services.User.Dto;

public class PagedUserDto
{
    public List<GetUserDto> Users { get; set; }
    public int TotalCount { get; set; }
    public int PageSize { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
}
