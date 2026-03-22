using DocumentFlowAPI.Data;
using DocumentFlowAPI.Interfaces.Repositories;
using DocumentFlowAPI.Repositories.Base;
using DocumentFlowAPI.Services.Template;
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

    public T UpdateTemplateStatus<T>(T template) where T : Models.Template
    {
        _dbContext.Attach(template);
        _dbContext.Entry(template)
            .Property(t => t.IsActive)
            .IsModified = true;
        return template;
    }

    public async Task<List<T>> GetAllTemplatesAsync<T>(TemplateFilter filter) where T : Models.Template
    {
        var query = _dbContext.Set<T>()
            .Include(t => t.User)
            .AsQueryable();

        if(filter.Title != null) query  = query.Where(t => t.Title.Contains(filter.Title));
        if(filter.CreatedBy.HasValue) query  = query.Where(t => t.CreatedBy == filter.CreatedBy.Value);
        if(filter.CreatedAt != null) query  = query.Where(t => t.CreatedAt == filter.CreatedAt);

        if (filter.PageSize.HasValue && filter.PageNumber.HasValue)
        {
            query = query
                .Skip((filter.PageNumber.Value - 1) *  filter.PageSize.Value)
                .Take(filter.PageSize.Value);;
        }
        
        return await query
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<T?> GetTemplateByIdAsync<T>(int templateId) where T : Models.Template
    {
        return await _dbContext.Set<T>().FindAsync(templateId);
    }

    public T UpdateTemplate<T>(T template) where T : Models.Template
    {
        UpdateFields(template,
            t => t.Title,
            t => t.Path,
            t => t.CreatedBy,
            t => t.CreatedAt,
            t => t.IsActive);
        return template;
    }

    public void DeleteTemplate<T>(T template) where T : Models.Template
    {
        _dbContext.Set<T>().Remove(template);
    }

    public async Task<int> GetTotalCountAsync<T>() where T : Models.Template
    {
        return await _dbContext.Set<T>().CountAsync();
    }
}