using FinanceApp.Shared.Models.UserTables;

namespace FinanceApp.EntityFramework
{
    public interface IRepository<TEntity> where TEntity : UserTable
    {
        Task<TEntity> InsertAsync(TEntity entity);
        TEntity Update(int id, TEntity entity);
        Task<TEntity> FirstOrDefaultAsync();
        void Remove(TEntity entity);
        Task<List<TEntity>> GetAllAsync();
        Task<TEntity> GetById(int id);
    }
}