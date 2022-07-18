using FinanceApp.Shared.Models.UserTables;
using FinanceApp.Shared.Models.UserTables.Bases;

namespace FinanceApp.EntityFramework
{
    public interface IRepository<TEntity> where TEntity : UserTable
    {
        Task<TEntity> InsertAsync(TEntity entity);
        Task<TEntity> UpdateAsync(int id, TEntity entity);
        Task<TEntity> FirstOrDefaultAsync();
        void Remove(TEntity entity);
        Task<List<TEntity>> GetAllListAsync();
        IQueryable<TEntity> GetAll();
        Task<TEntity> GetByIdAsync(int id);
    }
}