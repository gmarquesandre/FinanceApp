using FinanceApp.Shared;
using FinanceApp.Shared.Entities.UserTables;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace FinanceApp.EntityFramework.User
{

    public class Repository<TEntity> : IRepository<TEntity> where TEntity : UserTable
    {
        public IHttpContextAccessor _httpContextAccessor;
        public UserContext _context { get; set; }
        public Repository(UserContext context, IHttpContextAccessor httpContextAccessor)
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

        public async Task<TEntity> UpdateAsync(Guid id, TEntity entity)
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

        public async Task<TEntity?> FirstOrDefaultAsync()
        {
            var item = await _context.Set<TEntity>().AsNoTracking().Where(a => a.UserId == _httpContextAccessor.HttpContext.User.GetUserId()).FirstOrDefaultAsync();
            
            return item;
        }

        public async Task<List<TEntity>> GetAllListAsync()
        {
            int userId = _httpContextAccessor.HttpContext.User.GetUserId();
            var items = await _context.Set<TEntity>().Where(a => a.UserId == userId).AsNoTracking().ToListAsync();

            return items;
        }
        public void Remove(TEntity entity)
        {
            if (entity.UserId != _httpContextAccessor.HttpContext.User.GetUserId())
                throw new Exception("Usuário Inválido");

            _context.Set<TEntity>().Remove(entity);

            _context.SaveChanges();
        }

        public void RemoveBatch(List<Guid> ids)
        {

            var entities = GetAllAsync().Where(a => ids.Contains(a.Id));

            if (entities.Any(a => a.UserId != _httpContextAccessor.HttpContext.User.GetUserId()))
                throw new Exception("Usuário Inválido");

            _context.Set<TEntity>().RemoveRange(entities);

            _context.SaveChanges();
        }

        public async Task RemoveAll()
        {
            var entities = await GetAllListAsync();

            _context.Set<TEntity>().RemoveRange(entities);

            _context.SaveChanges();
        }

        public async Task<TEntity> GetByIdAsync(Guid id)
        {
            var item = await _context.Set<TEntity>().AsNoTracking().Where(a => a.UserId == _httpContextAccessor.HttpContext.User.GetUserId()).FirstOrDefaultAsync(a => a.Id == id);
            if (item == null)
                throw new Exception();
            return item;
        }
        

        public IQueryable<TEntity> GetAllAsync()
        {
            var items = _context.Set<TEntity>().AsNoTracking().Where(a => a.UserId == _httpContextAccessor.HttpContext.User.GetUserId());

            return items;
        }
    }
}
