using DocumentFlowAPI.Data;
using DocumentFlowAPI.Interfaces.Repositories;
using DocumentFlowAPI.Repositories.Base;

namespace DocumentFlowAPI.Repositories;

public class RoleRepository : BaseRepository<Models.Role>, IRoleRepository
{
    public RoleRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}
