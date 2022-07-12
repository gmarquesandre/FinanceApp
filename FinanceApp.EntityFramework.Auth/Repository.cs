using FinanceApp.Shared;
using FinanceApp.Shared.Models.UserTables;
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

        public TEntity Update(int id, TEntity entity)
        {
            entity.Id = id;
            entity.UserId = _httpContextAccessor.HttpContext.User.GetUserId();
            entity.UpdateDateTime = DateTime.Now; 
            var updatedRow = _context.Set<TEntity>().Update(entity).Entity;
            _context.SaveChanges();
            return updatedRow;

        }

        public async Task<TEntity> FirstOrDefaultAsync()
        {
            var item = (await _context.Set<TEntity>().AsNoTracking().FirstOrDefaultAsync());

            return item;
        }

        public async Task<List<TEntity>> GetAllAsync()
        {
            var items = await _context.Set<TEntity>().AsNoTracking().ToListAsync();

            return items;
        }
        public void Remove(TEntity entity)
        {

            _context.Set<TEntity>().Remove(entity);

            _context.SaveChanges();
        }
        
        public async Task<TEntity> GetById(int id)
        {
            var item = await _context.Set<TEntity>().AsNoTracking().FirstOrDefaultAsync(a => a.Id == id);

            return item;
        }
    }
}
