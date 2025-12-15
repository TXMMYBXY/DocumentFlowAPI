using DocumentFlowAPI.Base;
using DocumentFlowAPI.Data;
using DocumentFlowAPI.Interfaces.Repositories;
using DocumentFlowAPI.Models.AboutUserModels;
using DocumentFlowAPI.Services.Auth.Dto;
using Microsoft.EntityFrameworkCore;

namespace DocumentFlowAPI.Repositories;

public class TokenRepository : BaseRepository<RefreshToken>, ITokenRepository
{
    private readonly ApplicationDbContext _dbContext; 
    public TokenRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task CreateRefreshTokenAsync(RefreshToken refreshToken)
    {
        await _dbContext.AddAsync(refreshToken);
    }

    public void DeleteRefreshToken(RefreshToken refreshToken)
    {
        _dbContext.RefreshTokens.Remove(refreshToken);
    }

    public async Task<RefreshToken?> GetRefreshTokenByUserIdAsync(int userId)
    {
        return await _dbContext.RefreshTokens.FirstOrDefaultAsync(t => t.UserId == userId);
    }
}
