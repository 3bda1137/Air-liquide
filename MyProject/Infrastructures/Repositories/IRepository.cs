using MyProject.Domain.Entities;
using System.Linq.Expressions;

namespace MyProject.Infrastructures.Repositories
{
    public interface IRepository<T> where T : BaseEntity, new()
    {
        void UpdateExcluding(T entity, params Expression<Func<T, object>>[] excludedProperties);

        IQueryable<T> Get(Expression<Func<T?, bool>> predicate = null);
        Task<T?> FirstOrDefaultAsync(Guid id, CancellationToken cancellationToken = default);

        Task<T?> FirstOrDefaultAsync(Expression<Func<T?, bool>> predicate = null,
          CancellationToken cancellationToken = default);
        //Task<IEnumerable<T?>> GetAllAsync(Expression<Func<T?, bool>> predicate = null, CancellationToken cancellationToken = default);
        Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
        Task<bool> ExistsAsync(Expression<Func<T?, bool>> predicate = null, CancellationToken cancellationToken = default);
        Task<int> CountAsync(Expression<Func<T, bool>> predicate = null, CancellationToken cancellationToken = default);
        Guid Add(T entity);
        void Update(T entity);
        void SaveIncluded(T entity, params string[] properties);
        void UpdatePartial(T entity, params Expression<Func<T, object>>[] updatedProperties);
        void Delete(T entity);
        Task<int> BatchDelete(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);

        void SaveChanges();
        Task SaveChangesAsync();
    }
}
