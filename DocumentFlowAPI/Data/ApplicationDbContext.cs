
using DocumentFlowAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace DocumentFlowAPI.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContextOptions) : base(dbContextOptions)
    {

    }
    public DbSet<Contract> Contracts { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Statement> Statements { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<ContractTemplate> ContractTemplates { get; set; }
    public DbSet<StatementTemplate> StatementTemplates { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}