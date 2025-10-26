using DocumentFlowAPI.Base;
using DocumentFlowAPI.Data;
using DocumentFlowAPI.Interfaces.Repositories;

namespace DocumentFlowAPI.Repositories.Statement
{
    public class StatementRepository : BaseRepository<Models.Statement>, IStatementRepository
    {
        public StatementRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}