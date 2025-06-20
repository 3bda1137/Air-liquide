using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using MyProject.Domain.Entities;
using MyProject.Infrastructures.DbContexts;
using System.Linq.Expressions;

namespace MyProject.Infrastructures.Repositories
{
    public class Repository<T> : IRepository<T> where T : BaseEntity, new()
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }
        public IQueryable<T> Get(Expression<Func<T?, bool>> predicate = null)
        {
            return predicate == null ? _dbSet.Where(entity => !entity.IsDeleted) : _dbSet.Where(entity => !entity.IsDeleted).Where(predicate);
        }
        public IQueryable<T?> GetNotDeleted()
        {
            return _dbSet.Where(e => !e.IsDeleted);
        }
        public async Task<T?> FirstOrDefaultAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await GetNotDeleted().FirstOrDefaultAsync(e => e.ID == id, cancellationToken).ConfigureAwait(false);
        }
        public async Task<T?> FirstOrDefaultAsync(Expression<Func<T?, bool>> predicate = null, CancellationToken cancellationToken = default)
        {
            return predicate == null ? await GetNotDeleted().FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false) : await GetNotDeleted().Where(predicate).FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);
        }


        public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await GetNotDeleted().AnyAsync(e => e.ID == id, cancellationToken).ConfigureAwait(false);
        }


        public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate = null, CancellationToken cancellationToken = default)
        {
            return predicate == null ? await GetNotDeleted().AnyAsync(cancellationToken) : await GetNotDeleted().AnyAsync(predicate, cancellationToken);
        }
        public async Task<int> CountAsync(Expression<Func<T, bool>> predicate = null, CancellationToken cancellationToken = default)
        {
            return predicate == null ? await GetNotDeleted().CountAsync(cancellationToken) : await GetNotDeleted().CountAsync(predicate, cancellationToken);
        }
        public Guid Add(T entity)
        {
            _dbSet.Add(entity);
            return entity.ID;
        }
        public void Update(T entity)
        {
            entity.UpdatedAt = DateTime.UtcNow;
            _dbSet.Attach(entity);

            var entry = _context.Entry(entity);
            entry.Property(e => e.CreatedAt).IsModified = false;
            entry.Property(e => e.CreatedBy).IsModified = false;
            entry.Property(e => e.UpdatedBy).IsModified = true;
            entry.Property(e => e.UpdatedAt).IsModified = true;
            entry.State = EntityState.Modified;
        }

        public void Delete(T entity)
        {
            entity.IsDeleted = true;
            entity.UpdatedAt = DateTime.UtcNow;

            var entry = _context.Entry(entity);
            entry.Property(e => e.CreatedAt).IsModified = false;
            entry.Property(e => e.CreatedBy).IsModified = false;
            entry.Property(e => e.IsDeleted).IsModified = true;
            entry.Property(e => e.UpdatedBy).IsModified = true;
            entry.Property(e => e.UpdatedAt).IsModified = true;
        }
        public virtual void SaveIncluded(T entity, params string[] properties)
        {
            var local = _context.Set<T>()
                .Local.FirstOrDefault(entry => entry.ID == entity.ID);

            EntityEntry entry;

            if (local is null)
                entry = _context.Entry(entity);
            else
                entry = _context.ChangeTracker.Entries<T>().FirstOrDefault(e => e.Entity.ID == entity.ID);

            foreach (var prop in entry.Properties)
            {
                if (properties.Contains(prop.Metadata.Name))
                {
                    prop.CurrentValue = entity.GetType().GetProperty(prop.Metadata.Name).GetValue(entity);
                    prop.IsModified = true;
                }
            }

            entity.UpdatedAt = DateTime.UtcNow;
        }
        public void UpdatePartial(T entity, params Expression<Func<T, object>>[] updatedProperties)
        {
            entity.UpdatedAt = DateTime.UtcNow;
            _dbSet.Attach(entity);

            _context.Entry(entity).Property(e => e.UpdatedBy).IsModified = true;
            _context.Entry(entity).Property(e => e.UpdatedAt).IsModified = true;

            foreach (var property in updatedProperties)
            {
                _context.Entry(entity).Property(property).IsModified = true;
            }
        }
        public void UpdateExcluding(T entity, params Expression<Func<T, object>>[] excludedProperties)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            entity.UpdatedAt = DateTime.UtcNow;

            if (_context.Entry(entity).State == EntityState.Detached)
                _dbSet.Attach(entity);

            var entry = _context.Entry(entity);

            // Always exclude these properties from modification
            var alwaysExcluded = new List<string>
        {
            nameof(BaseEntity.ID),
            nameof(BaseEntity.CreatedAt),
            nameof(BaseEntity.CreatedBy)
        };

            // Add navigation properties that are part of the key
            var keyProperties = entry.Metadata.FindPrimaryKey()?.Properties
                .Select(p => p.Name)
                .ToList() ?? new List<string>();

            alwaysExcluded.AddRange(keyProperties);

            // Get names of properties to exclude from parameters
            var excludedPropertyNames = excludedProperties
                .Select(GetPropertyName)
                .Concat(alwaysExcluded)
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            // Force update of audit fields
            entry.Property(e => e.UpdatedAt).IsModified = true;
            entry.Property(e => e.UpdatedBy).IsModified = true;

            // Mark all non-excluded properties as modified
            foreach (var property in entry.Properties)
            {
                property.IsModified = !excludedPropertyNames.Contains(property.Metadata.Name);
            }
        }

        private string GetPropertyName<TProperty>(Expression<Func<T, TProperty>> propertyExpression)
        {
            var memberExpression = propertyExpression.Body as MemberExpression;
            if (memberExpression == null)
            {
                var unaryExpression = propertyExpression.Body as UnaryExpression;
                if (unaryExpression != null)
                {
                    memberExpression = unaryExpression.Operand as MemberExpression;
                }
            }
            return memberExpression?.Member.Name;
        }


        public async Task<int> BatchDelete(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await GetNotDeleted().Where(predicate).ExecuteDeleteAsync(cancellationToken).ConfigureAwait(false);
        }
        public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
        {
            return await Get().AnyAsync(predicate).ConfigureAwait(false);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }
    }

}
