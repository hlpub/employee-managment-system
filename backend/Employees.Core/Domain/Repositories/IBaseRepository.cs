using Employees.Core.DbContexts;
using Employees.Core.Domain.Models;

namespace Employees.Core.Domain.Repositories
{
    public interface IBaseRepository<T> where T : ModelBase
    {
        Task<IEnumerable<T>> SearchAsync(Func<T, bool> searchPredicate, CancellationToken cancellationToken);
        Task<IEnumerable<T>> GetAsync(CancellationToken cancellationToken);
        Task<T> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<int> CreateAsync(T entity, CancellationToken cancellationToken);
        Task DeleteAsync(int id, CancellationToken cancellationToken);
        Task SaveChangesAsync(CancellationToken cancellationToken);
    }
}