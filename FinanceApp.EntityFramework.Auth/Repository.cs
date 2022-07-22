using FinanceApp.Shared;
using FinanceApp.Shared.Models.UserTables;
using FinanceApp.Shared.Models.UserTables.Bases;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

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
            entity.CreationDateTime = DateTime.Now;
            entity.UpdateDateTime = DateTime.Now;
            var newRow = (await _context.Set<TEntity>().AddAsync(entity)).Entity;
            await _context.SaveChangesAsync();
            return newRow;
        }

        public async Task<TEntity> UpdateAsync(int id, TEntity entity)
        {
            entity.Id = id;

            var oldEntity = await GetByIdAsync(id);

            entity.UserId = _httpContextAccessor.HttpContext.User.GetUserId();
            entity.UpdateDateTime = DateTime.Now;
            entity.CreationDateTime = oldEntity.CreationDateTime;
            var updatedRow = _context.Set<TEntity>().Update(entity).Entity;
            await _context.SaveChangesAsync();
            return updatedRow;

        }

        public async Task<TEntity> FirstOrDefaultAsync()
        {
            var item = (await _context.Set<TEntity>().AsNoTracking().FirstOrDefaultAsync());

            return item;
        }

        public async Task<List<TEntity>> GetAllListAsync()
        {
            var items = await _context.Set<TEntity>().AsNoTracking().ToListAsync();

            return items;
        }
        public void Remove(TEntity entity)
        {

            _context.Set<TEntity>().Remove(entity);

            _context.SaveChanges();
        }
        
        public async Task<TEntity> GetByIdAsync(int id)
        {
            var item = await _context.Set<TEntity>().AsNoTracking().FirstOrDefaultAsync(a => a.Id == id);

            return item;
        }

        public IQueryable<TEntity> GetAll()
        {
            var items = _context.Set<TEntity>().AsNoTracking();

            return items;
        }
    }
}
