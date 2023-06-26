using Employees.Core.Domain.Models;

namespace Employees.Core.Services
{
    public interface IEmployeeService
    {
        Task<IEnumerable<Employee>> SearchAsync(string search,
            string adminKey, CancellationToken cancellationToken);
        bool IsValid(Employee employee);
        Task UpdateEmployeeAsync(Employee employee, Employee updatedEmployee, CancellationToken cancellationToken);
    }
}