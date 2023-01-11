using System.Linq.Expressions;

namespace UTP_Web_API.Repository.IRepository
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? filter = null);
        Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> filter, bool tracked = true);
        Task<bool> ExistAsync(Expression<Func<TEntity, bool>> filter);
        Task<TEntity> CreateAsync(TEntity entity);
        Task RemoveAsync(TEntity entity);
        Task Update(TEntity entity);
        Task SaveAsync();
    }
}
