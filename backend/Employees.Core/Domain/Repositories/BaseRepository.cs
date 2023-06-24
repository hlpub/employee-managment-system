﻿using Employees.Core.DbContexts;
using Employees.Core.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Employees.Core.Domain.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : ModelBase
    {
        private readonly EmployeesDbContext _employeesDbContext;

        public BaseRepository(EmployeesDbContext employeesDbContext)
        {
            ArgumentNullException.ThrowIfNull(employeesDbContext, nameof(employeesDbContext));

            _employeesDbContext = employeesDbContext;
        }
        public virtual async Task<T> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _employeesDbContext.Set<T>().FindAsync(id, cancellationToken);
        }

        public virtual async Task<IEnumerable<T>> GetAsync(CancellationToken cancellationToken)
        {
            return await _employeesDbContext.Set<T>().AsQueryable().AsNoTracking().ToListAsync(cancellationToken);
        }

        public virtual async Task<IEnumerable<T>> SearchAsync(Func<T, bool> searchPredicate, CancellationToken cancellationToken)
        {
            return await _employeesDbContext.Set<T>().Where(searchPredicate)
                .AsQueryable().AsNoTracking().ToListAsync(cancellationToken);
        }

        public virtual async Task<int> CreateAsync(T entity, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(entity, nameof(entity));

            _employeesDbContext.Set<T>().Add(entity);

            await _employeesDbContext.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }

        public virtual async Task DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var objEmployee = await GetByIdAsync(id, cancellationToken);

            if (objEmployee is null)
                throw new Exception($"Delete failed. No employee was found for ID {id}");

            _employeesDbContext.Remove(objEmployee);
            await _employeesDbContext.SaveChangesAsync(cancellationToken);
        }

        public virtual async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await _employeesDbContext?.SaveChangesAsync(cancellationToken);
        }
    }
}
