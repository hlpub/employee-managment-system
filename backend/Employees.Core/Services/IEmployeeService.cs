using Employees.Core.Domain.Models;

namespace Employees.Core.Services
{
    public interface IEmployeeService
    {
        Task<IEnumerable<Employee>> SearchAsync(string search, CancellationToken cancellationToken);
    }
}