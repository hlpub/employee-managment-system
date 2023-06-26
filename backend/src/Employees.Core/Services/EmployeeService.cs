using Employees.Core.Domain.Models;
using Employees.Core.Domain.Repositories;
using Microsoft.IdentityModel.Tokens;
using System.Net.Mail;


namespace Employees.Core.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IBaseRepository<Employee> _employeeRepository;

        public EmployeeService(IBaseRepository<Employee> employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<IEnumerable<Employee>> SearchAsync(string search,string adminKey, CancellationToken cancellationToken)
        {
            IEnumerable<Employee> employees;

            if (!search.IsNullOrEmpty())
            {
                employees = await _employeeRepository.SearchAsync(x => x.FirstName.Contains(search) ||
                    x.LastName.Contains(search) || x.JobTitle.Contains(search), cancellationToken);
            }
            else
            {
                employees = await _employeeRepository.GetAsync(cancellationToken);
            }

            return employees;
        }

        public bool IsValid(Employee employee)
        {
            if (employee is null || employee.JobTitle.IsNullOrEmpty()
                || employee.FirstName.IsNullOrEmpty() || employee.LastName.IsNullOrEmpty()
                || employee.Email.IsNullOrEmpty() || !IsValidMail(employee.Email))
                return false;


            return true;
        }

        bool IsValidMail(string emailAddress)
        {
            try
            {
                _ = new MailAddress(emailAddress);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task UpdateEmployeeAsync(Employee employee, Employee updatedEmployee, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(employee, nameof(employee));
            ArgumentNullException.ThrowIfNull(updatedEmployee, nameof(updatedEmployee));

            employee.Email = updatedEmployee.Email;
            employee.JobTitle = updatedEmployee.JobTitle;
            employee.DateOfJoining = updatedEmployee.DateOfJoining;
            employee.FirstName = updatedEmployee.FirstName;
            employee.LastName = updatedEmployee.LastName;

            await _employeeRepository.SaveChangesAsync(cancellationToken);
        }
    }
}
