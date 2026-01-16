using DocumentFlowAPI.Base;
using DocumentFlowAPI.Data;
using DocumentFlowAPI.Interfaces.Repositories;
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

    public async Task<List<T>> GetAllTemplatesAsync<T>() where T : Models.Template
    {
        return await _dbContext.Set<T>().ToListAsync();
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
}