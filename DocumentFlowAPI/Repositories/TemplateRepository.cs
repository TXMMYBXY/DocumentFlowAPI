using DocumentFlowAPI.Data;
using DocumentFlowAPI.Interfaces.Repositories;
using DocumentFlowAPI.Repositories.Base;
using DocumentFlowAPI.Services.Template;
using DocumentFlowAPI.Services.Template.Dto;
using Microsoft.EntityFrameworkCore;

namespace DocumentFlowAPI.Repositories.Template;

public class TemplateRepository : BaseRepository<Models.Template>, ITemplateRepository
{
    private readonly ApplicationDbContext _dbContext;

    public TemplateRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task CreateTemplateAsync<T>(T template) where T : Models.Template
    {
        await _dbContext.Set<T>().AddAsync(template);
    }

    public async Task<bool> UpdateTemplateStatusAsync<T>(int templateId) where T : Models.Template
    {
        await _dbContext.Set<T>()
            .Where(t => t.Id == templateId)
            .ExecuteUpdateAsync(setters =>
                setters.SetProperty(t => t.IsActive, t => !t.IsActive)
            );

        return await _dbContext.Set<T>()
            .Where(t => t.Id == templateId)
            .Select(t => t.IsActive)
            .FirstAsync();
    }

    public async Task<List<T>> GetAllTemplatesAsync<T>(TemplateFilter filter) where T : Models.Template
    {
        var query = _dbContext.Set<T>()
            .Include(t => t.User)
            .AsQueryable();

        if (filter.Title != null) query = query.Where(t => t.Title.Contains(filter.Title));
        if (filter.CreatedBy.HasValue) query = query.Where(t => t.CreatedBy == filter.CreatedBy.Value);
        if (filter.CreatedAtEarlier != null) query = query.Where(t => t.CreatedAt <= filter.CreatedAtEarlier.Value);
        if (filter.CreatedAtLater != null) query = query.Where(t => t.CreatedAt >= filter.CreatedAtLater.Value);

        if (filter.PageSize.HasValue && filter.PageNumber.HasValue)
        {
            query = query
                .Skip((filter.PageNumber.Value - 1) * filter.PageSize.Value)
                .Take(filter.PageSize.Value);
        }

        return await query
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<T?> GetTemplateForWorkerByIdAsync<T>(int templateId) where T : Models.Template
    {
        return await _dbContext.Set<T>().FindAsync(templateId);
    }

    public async Task UpdateTemplatePartialAsync<T>(int templateId, string? title, string? path) where T : Models.Template
    {
        var query = _dbContext.Set<T>()
            .Where(t => t.Id == templateId);

        if (title != null && path != null)
        {
            await query.ExecuteUpdateAsync(s => s
                .SetProperty(t => t.Title, title)
                .SetProperty(t => t.Path, path));
        }
        else if (title != null)
        {
            await query.ExecuteUpdateAsync(s => s
                .SetProperty(t => t.Title, title));
        }
        else if (path != null)
        {
            await query.ExecuteUpdateAsync(s => s
                .SetProperty(t => t.Path, path));
        }
    }

    public async Task<int> GetTotalCountAsync<T>() where T : Models.Template
    {
        return await _dbContext.Set<T>().CountAsync();
    }

    public async Task DeleteManyTemplatesAsync<T>(List<int> templateIds) where T : Models.Template
    {
        await _dbContext.Set<T>()
            .Where(t => templateIds.Contains(t.Id))
            .ExecuteDeleteAsync();
    }

    public async Task DeleteTemplateAsync<T>(int templateId) where T : Models.Template
    {
        await _dbContext.Set<T>()
            .Where(t => t.Id == templateId)
            .ExecuteDeleteAsync();
    }

    public async Task<T> GetTemplateByIdAsync<T>(int templateId) where T : Models.Template
    {
        return await _dbContext.Set<T>().FindAsync(templateId);
    }

    public async Task<WorkerTemplateDto> GetWorkerTemplateByIdAsync<T>(int templateId) where T : Models.Template
    {
        return await _dbContext.Set<T>()
            .Where(t => t.Id == templateId)
            .Select(t => new WorkerTemplateDto
            {
                Id = t.Id,
                Name = t.Title,
                FilePath = t.Path
            })
            .SingleAsync();
    }

    public async Task<string> GetFilePathAsync<T>(int templateId) where T : Models.Template
    {
        return await _dbContext.Set<T>()
            .Where(t => t.Id == templateId)
            .Select(t => t.Path)
            .FirstAsync();
    }
}