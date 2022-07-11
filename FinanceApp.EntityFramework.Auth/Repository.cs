using FinanceApp.Api;
using FinanceApp.Shared.Models.UserTables;
using Microsoft.AspNetCore.Http;

namespace FinanceApp.EntityFramework
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : UserTable
    {
        public IHttpContextAccessor _httpContextAccessor;
        public FinanceContext _context { get; set; }
        public Repository(FinanceContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<TEntity> InsertAsync(TEntity entity)
        {
            entity.UserId = _httpContextAccessor.HttpContext.User.GetUserId();
            var newRow = (await _context.Set<TEntity>().AddAsync(entity)).Entity;
            await _context.SaveChangesAsync();
            return newRow;
        }

        public TEntity Update(TEntity entity)
        {
            entity.UserId = _httpContextAccessor.HttpContext.User.GetUserId();
            var updatedRow = _context.Set<TEntity>().Update(entity).Entity;
            _context.SaveChanges();
            return updatedRow;

        }
    }
}
