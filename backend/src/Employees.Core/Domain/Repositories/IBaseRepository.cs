using Employees.Core.Domain.Models;
using System.Linq.Expressions;

namespace Employees.Core.Domain.Repositories
{
    public interface IBaseRepository<T> where T : ModelBase
    {
        Task<IEnumerable<T>> SearchAsync(Expression<Func<T, bool>> searchPredicate, CancellationToken cancellationToken);
        Task<IEnumerable<T>> GetAsync(CancellationToken cancellationToken);
        Task<bool> AnyAsync(Expression<Func<T, bool>> searchPredicate, CancellationToken cancellationToken);
        Task<T> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<int> CreateAsync(T entity, CancellationToken cancellationToken);
        Task DeleteAsync(int id, CancellationToken cancellationToken);
        Task SaveChangesAsync(CancellationToken cancellationToken);
    }
}