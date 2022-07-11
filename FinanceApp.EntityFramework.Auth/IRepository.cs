using FinanceApp.Shared.Models.UserTables;

namespace FinanceApp.EntityFramework
{
    public interface IRepository<TEntity> where TEntity : UserTable
    {
        Task<TEntity> InsertAsync(TEntity entity);
        TEntity Update(TEntity model);
    }
}