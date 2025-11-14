using DocumentFlowAPI.Base;
using DocumentFlowAPI.Data;
using DocumentFlowAPI.Interfaces.Repositories;

namespace DocumentFlowAPI.Repositories.Token;

public class TokenRepository : BaseRepository<Models.Token>, ITokenRepository
{
    public TokenRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}