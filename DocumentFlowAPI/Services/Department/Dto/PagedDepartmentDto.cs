using DocumentFlowAPI.Services.General;

namespace DocumentFlowAPI.Services.Department.Dto;

public class PagedDepartmentDto : PagedData
{
    public List<GetDepartmentDto> Departments { get; set; }
}
