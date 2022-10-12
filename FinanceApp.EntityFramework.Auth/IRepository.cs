using FinanceApp.Shared.Entities.UserTables;

namespace FinanceApp.EntityFramework.User
{
    public interface IRepository<TEntity> where TEntity : UserTable
    {
        Task<TEntity> InsertAsync(TEntity entity);
        Task<TEntity> UpdateAsync(int id, TEntity entity);
        Task<TEntity?> FirstOrDefaultAsync();
        void Remove(TEntity entity);
        Task<List<TEntity>> GetAllListAsync();
        IQueryable<TEntity> GetAllAsync();
        void RemoveBatch(List<int> ids);
        Task<TEntity> GetByIdAsync(int id);
        Task RemoveAll();
    }
}