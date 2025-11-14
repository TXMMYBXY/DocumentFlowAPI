using DocumentFlowAPI.Base;
using DocumentFlowAPI.Data;
using DocumentFlowAPI.Interfaces.Repositories;

namespace DocumentFlowAPI.Repositories.Contract;

public class ContractRepository : BaseRepository<Models.Contract>, IContractRepository
{
    public ContractRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}